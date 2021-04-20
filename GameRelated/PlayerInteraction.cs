﻿using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Scrap_Scramble_Final_Version.GameRelated.Cards;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated
{
    public class PlayerInteraction
    {
        public string title;
        public string description;
        public string footer;

        public PlayerInteraction()
        {
            this.title = string.Empty;
            this.description = string.Empty;
            this.footer = string.Empty;
        }
        public PlayerInteraction(string title, string description, string footer)
        {
            this.title = title;
            this.description = description;
            this.footer = footer;
        }

        public delegate bool inputCriteria(string input, GameHandler gameHandler, ulong curPlayer);

        public async Task<string> SendInteractionAsync(GameHandler gameHandler, ulong curPlayer, inputCriteria criteria, string timeOutAnswer, CommandContext ctx)
        {
            if (!gameHandler.players.ContainsKey(curPlayer)) return string.Empty;

            //BotInfoHandler.RefreshUI(gameHandler.players[curPlayer].ctx, curPlayer);

            var interactivity = ctx.Client.GetInteractivity();

            var embedMsg = await ctx.Channel.SendMessageAsync(embed: new DiscordEmbedBuilder
            {
                Title = this.title,
                Description = this.description,
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = this.footer }
            }).ConfigureAwait(false);

            while (true)
            {
                var result = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel &&
                                                                          x.Author == ctx.User).ConfigureAwait(false);

                if (result.TimedOut)
                {
                    //apply default answer
                    await embedMsg.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":clock4:"));
                    await ctx.Channel.SendMessageAsync(embed: new DiscordEmbedBuilder
                    {
                        Title = "Your Interactive Prompt Timed Out",
                        Description = $"As an input has been chosen {timeOutAnswer}.",
                        Color = DiscordColor.Yellow
                    }).ConfigureAwait(false);
                    return timeOutAnswer;
                }

                if (criteria(result.Result.Content, gameHandler, curPlayer))
                {
                    //message fits the criteria

                    await result.Result.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:"));
                    return result.Result.Content;
                }
                else
                {
                    await result.Result.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:"));
                }
            }
        }

        //public async Task<string> SendInteractionAsync(int index, bool showEmbed = true)
        //{
        //    if (index < 0 || index >= BotInfoHandler.gameHandler.players.Count()) return string.Empty;

        //    BotInfoHandler.RefreshUI(BotInfoHandler.gameHandler.players[index].ctx, index);

        //    var interactivity = BotInfoHandler.gameHandler.players[index].ctx.Client.GetInteractivity();

        //    if (showEmbed) await BotInfoHandler.gameHandler.players[index].ctx.RespondAsync(embed: new DiscordEmbedBuilder
        //    {
        //        Title = this.title,
        //        Description = this.description,
        //        Color = DiscordColor.Azure,
        //        Footer = new DiscordEmbedBuilder.EmbedFooter { Text = this.footer }
        //    }).ConfigureAwait(false);

        //    var result = await interactivity.WaitForMessageAsync(x => x.Channel == BotInfoHandler.gameHandler.players[index].ctx.Channel && 
        //                                                         x.Author == BotInfoHandler.gameHandler.players[index].ctx.User).ConfigureAwait(false);            

        //    if (result.TimedOut) return "TimedOut";

        //    string msg = result.Result.Content;
        //    int dummyint = 0;

        //    switch (this.answerType)
        //    {
        //        case AnswerType.IntAnswer:
        //            if (int.TryParse(msg, out dummyint)) return msg;
        //            else return string.Empty;

        //        case AnswerType.CharAnswer:
        //            if (msg.Length == 1) return msg;
        //            else return string.Empty;

        //        case AnswerType.StringAnswer:
        //            if (msg == "TimedOut") return string.Empty;
        //            return msg;

        //        default:
        //            return string.Empty;
        //    }
        //}

        private static bool CheckValidShopUpgradeIndex(string s, GameHandler gameHandler, ulong curPlayer)
        {
            if (int.TryParse(s, out int ret))
            {
                if (!(1 <= ret && ret <= gameHandler.players[curPlayer].shop.LastIndex)) return false;
                return gameHandler.players[curPlayer].shop.At(ret - 1).name != BlankUpgrade.name;
            }
            else return false;
        }

        public static async Task<int> ChooseUpgradeInShopAsync(GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return -1;

            string defaultAns = (gameHandler.players[curPlayer].shop.GetRandomUpgradeIndex() + 1).ToString();

            PlayerInteraction interaction = new PlayerInteraction("Choose an Upgrade in your shop", string.Empty, "Write the corresponding index");

            string ret = await interaction.SendInteractionAsync(gameHandler, curPlayer, PlayerInteraction.CheckValidShopUpgradeIndex, defaultAns, ctx);
            if (ret == string.Empty) return -1;

            int pos = int.Parse(ret) - 1;
            //Upgrade chosen = gameHandler.players[curPlayer].shop.At(pos);

            //return chosen;
            return pos;
        }

        public static async Task<Upgrade> FreezeUpgradeInShopAsync(GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx, int freezeAmount = 1)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return new BlankUpgrade();
            string defaultAns = (gameHandler.players[curPlayer].shop.GetRandomUpgradeIndex() + 1).ToString();

            PlayerInteraction interaction = new PlayerInteraction("Choose an Upgrade in your shop to Freeze", string.Empty, "Write the corresponding index");

            string ret = await interaction.SendInteractionAsync(gameHandler, curPlayer, PlayerInteraction.CheckValidShopUpgradeIndex, defaultAns, ctx);
            if (ret == string.Empty) return new BlankUpgrade();

            int pos = int.Parse(ret) - 1;
            Upgrade chosen = gameHandler.players[curPlayer].shop.At(pos);

            chosen.creatureData.staticKeywords[StaticKeyword.Freeze] = Math.Max(freezeAmount, chosen.creatureData.staticKeywords[StaticKeyword.Freeze]);
            //chosen.OnBeingFrozen(gameHandler, curPlayer, enemy);

            var generalInfo = new ExtraEffectInfo(ctx);
            await Effect.CallEffects(chosen.effects, Effect.EffectType.OnBeingFrozen, chosen, gameHandler, curPlayer, enemy, generalInfo);

            return chosen;
        }

        public static async Task<int> ActivateMagneticAsync(GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
        {
            PlayerInteraction magnetic = new PlayerInteraction("Write the number of a Spare Part", string.Empty, "");
            string defaultAns = (GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].pool.spareparts.Count()) + 1).ToString();

            for (int i = 0; i < gameHandler.players[curPlayer].pool.spareparts.Count(); i++)
            {
                magnetic.description += $"{i + 1}) {gameHandler.players[curPlayer].pool.spareparts[i].GetInfo(gameHandler, curPlayer)}";
                if (i != gameHandler.players[curPlayer].pool.spareparts.Count() - 1) magnetic.description += "\n";
            }

            string ret = await magnetic.SendInteractionAsync(gameHandler, curPlayer, (x, y, z) => GeneralFunctions.Within(x, 1, y.pool.spareparts.Count()), defaultAns, ctx);
            if (ret == string.Empty) return -1;

            int index = int.Parse(ret) - 1;
            gameHandler.players[curPlayer].hand.AddCard(gameHandler.players[curPlayer].pool.spareparts[index]);

            return index;
        }

        public static async Task<Card> DiscoverACardAsync<T>(GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx, string promptTitle, List<T> cardPool, bool addToHand = true) where T : Card
        {
            List<int> cardIndexes = new List<int>();

            if (cardPool.Count() <= 3)
            {
                if (cardPool.Count() == 0) return new BlankUpgrade();

                for (int i = 0; i < cardPool.Count(); i++)
                {
                    cardIndexes.Add(i);
                }
            }
            else
            {
                cardIndexes.Add(GameHandler.randomGenerator.Next(0, cardPool.Count()));
                cardIndexes.Add(GameHandler.randomGenerator.Next(0, cardPool.Count() - 1));
                cardIndexes.Add(GameHandler.randomGenerator.Next(0, cardPool.Count() - 2));

                if (cardIndexes[1] >= cardIndexes[0]) cardIndexes[1]++;
                if (cardIndexes[2] >= Math.Min(cardIndexes[0], cardIndexes[1])) cardIndexes[2]++;
                if (cardIndexes[2] >= Math.Max(cardIndexes[0], cardIndexes[1])) cardIndexes[2]++;
            }

            string desc = string.Empty;

            for (int i = 0; i < cardIndexes.Count(); i++)
            {
                desc += $"{i + 1}) {cardPool.ElementAt(cardIndexes[i]).GetInfo(gameHandler, curPlayer)}\n";
            }

            PlayerInteraction interaction = new PlayerInteraction($"{promptTitle}", desc, $"Write the corresponding index between 1 and {cardIndexes.Count()}");

            string defaultAns = GameHandler.randomGenerator.Next(1, cardIndexes.Count() + 1).ToString();
            string ret = await interaction.SendInteractionAsync(gameHandler, curPlayer, (x, y, z) => GeneralFunctions.Within(x, 1, cardIndexes.Count()), defaultAns, ctx);

            if (addToHand)
            {
                int handIndex = gameHandler.players[curPlayer].hand.AddCard(cardPool.ElementAt(cardIndexes[int.Parse(ret) - 1]));
                return gameHandler.players[curPlayer].hand.At(handIndex);
            }
            else
            {
                return cardPool.ElementAt(cardIndexes[int.Parse(ret) - 1]).DeepCopy();
            }
        }
    }
}
