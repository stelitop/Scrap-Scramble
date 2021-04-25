using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Emzi0767.Utilities;
using Scrap_Scramble_Final_Version.GameRelated;
using Scrap_Scramble_Final_Version.GameRelated.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.BotRelated.Commands.GameCommands
{
    public class InformationCommands : BaseCommandModule
    {
        [Group("lookup")]
        public class LookupCommands : BaseCommandModule
        {
            private DiscordEmbedBuilder FindCard<T>(CommandContext ctx, List<T> pool, string cardName) where T : Card
            {
                DiscordEmbedBuilder ret = new DiscordEmbedBuilder();

                if (cardName.Count() == 0)
                {
                    ret = new DiscordEmbedBuilder{
                        Title = "Incomplete command",
                        Description = "You need to follow up the command with the name of the Card.",
                        Color = DiscordColor.Yellow
                    };

                    return ret;
                }

                if (cardName.Length < 3)
                {
                    ret = new DiscordEmbedBuilder{
                        Title = "Input Is Too Short",
                        Description = "Your input needs to be at least 3 characters long.",
                        Color = DiscordColor.Red
                    };
                    return ret;
                }

                List<int> candidates = new List<int>();

                for (int i = 0; i < pool.Count(); i++)
                {
                    if (pool[i].name.ToLower().Contains(cardName.ToLower()))
                    {
                        candidates.Add(i);
                    }
                }

                if (candidates.Count() == 1)
                {
                    Card card = pool[candidates[0]];


                    ret = new DiscordEmbedBuilder
                    {
                        Title = $"{card.name}",
                        Color = DiscordColor.Green
                    };

                    if (card is Upgrade u)
                    {
                        ret.Description = $"{u.Cost}/{u.creatureData.attack}/{u.creatureData.health} - {u.rarity} Upgrade";
                        if (u.upgradeSet != UpgradeSet.None)
                        {
                            ret.Description += $" | {SetHandler.SetAttributeToString[u.upgradeSet]}";
                        }
                    }
                    if (card is Spell s)
                    {
                        ret.Description = $"{s.Cost}-Cost Spell";
                    }
                    

                    if (!card.cardText.Equals(string.Empty)) ret.Description += $"\n{card.cardText}";
                   
                    return ret;
                }
                else if (candidates.Count() > 1)
                {
                    ret = new DiscordEmbedBuilder
                    {
                        Title = $"{candidates.Count()} Matches Found",
                        Color = DiscordColor.Azure
                    };

                    for (int i = 0; i < candidates.Count(); i++)
                    {
                        if (i == 0) ret.Description = pool[candidates[i]].name;
                        else ret.Description += $", {pool[candidates[i]].name}";
                    }

                    return ret;
                }

                ret = new DiscordEmbedBuilder
                {
                    Title = "No Matches Found",
                    Description = "Please make sure you typed the name correctly.",
                    Color = DiscordColor.Red
                };

                return ret;
            }

            [Aliases("u")]
            [Command("upgrade")]
            public async Task LookupUpgrade(CommandContext ctx, [Description("Name of the Upgrade")][RemainingText] string cardName)
            {
                var embed = FindCard<Upgrade>(ctx, BotHandler.generalCardPool.upgrades, cardName);
                await ctx.RespondAsync(embed: embed.Build()).ConfigureAwait(false);
                return;
            }

            [Aliases("t")]
            [Command("token")]
            public async Task LookupToken(CommandContext ctx, [Description("Name of the Token")][RemainingText] string cardName)
            {
                var embed = FindCard<Card>(ctx, BotHandler.generalCardPool.tokens, cardName);
                await ctx.RespondAsync(embed: embed.Build()).ConfigureAwait(false);
                return;
            }

            [Command("sets")]
            public async Task ShowListOfSets(CommandContext ctx) => await ShowInteractiveSetViewer(ctx);
            //{
            //    string msg = string.Empty;

            //    int i = 1;
                
            //    foreach (var key in BotHandler.setHandler.Sets.Keys)
            //    {
            //        msg += $"{i}) {key}\n";
            //        i++;
            //    }

            //    await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            //    {
            //        Title = "List of Sets",
            //        Description = msg,
            //        Color = DiscordColor.Azure
            //    }).ConfigureAwait(false);
            //}


            private DiscordEmbedBuilder GetSetMessageEmbed(List<string> sets, int curSet, Rarity rarity)
            {

                DiscordEmbedBuilder ret = new DiscordEmbedBuilder();
                
                if (!BotHandler.setHandler.Sets.ContainsKey(sets[curSet]))
                {
                    ret = new DiscordEmbedBuilder
                    {
                        Title = "Unregistered Set Name Found",
                        Description = "You shouldn't be seeing this message. This is a bug.",
                        Color = DiscordColor.Violet
                    };

                    return ret;
                }

                ret.Title = $"{sets[curSet]} - Set Info";
                ret.Color = DiscordColor.Azure;

                string upgradeInfoField = string.Empty;

                switch (rarity)
                {
                    case Rarity.Common:
                    case Rarity.Rare:
                    case Rarity.Epic:
                    case Rarity.Legendary:
                        foreach (var u in BotHandler.setHandler.Sets[sets[curSet]])
                        {
                            if (u.rarity == rarity)
                            {
                                upgradeInfoField += $"- {u}\n";
                            }
                        }
                        ret.AddField($"{rarity} Upgrades", upgradeInfoField);
                        break;

                    default:
                        ret.AddField("Set Description", "To Be Done. This is where information about a set's flavour and its main mechanics will be explained.");
                        break;
                }

                int prevSet = curSet - 1;
                int nextSet = curSet + 1;
                if (prevSet < 0) prevSet += sets.Count();
                if (nextSet >= sets.Count()) nextSet -= sets.Count();

                ret.AddField(":arrow_left:", $"{sets[prevSet]}", true);

                string nextFieldText = string.Empty;

                switch(rarity)
                {
                    case Rarity.Common:
                        nextFieldText = "Rare Upgrades";
                        break;
                    case Rarity.Rare:
                        nextFieldText = "Epic Upgrades";
                        break;
                    case Rarity.Epic:
                        nextFieldText = "Legendary Upgrades";
                        break;
                    case Rarity.Legendary:
                        nextFieldText = "Set Description";
                        break;
                    default:
                        nextFieldText = "Common Upgrades";
                        break;
                }

                ret.AddField(":arrows_counterclockwise:", nextFieldText, true);
                ret.AddField(":arrow_right:", $"{sets[nextSet]}", true);

                return ret;
            }
            public async Task ShowInteractiveSetViewer(CommandContext ctx)
            {
                List<string> sets = BotHandler.setHandler.Sets.Keys.ToList();
                List<Rarity> curRarities = new List<Rarity>();

                foreach (var x in sets) curRarities.Add(Rarity.NO_RARITY);

                int curSet = 0;                

                var interactivity = ctx.Client.GetInteractivity();

                var setMessage = await ctx.RespondAsync(embed: GetSetMessageEmbed(sets, curSet, curRarities[curSet]).Build()).ConfigureAwait(false);

                List<DiscordEmoji> emotes = new List<DiscordEmoji> { 
                    DiscordEmoji.FromName(ctx.Client, ":arrow_left:"),
                    DiscordEmoji.FromName(ctx.Client, ":arrows_counterclockwise:"),
                    DiscordEmoji.FromName(ctx.Client, ":arrow_right:"),
                    DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:"),
                };

                for (int i=0; i<emotes.Count; i++)
                {
                    await setMessage.CreateReactionAsync(emotes[i]).ConfigureAwait(false);
                }

                while (true)
                {
                    var result = await interactivity.WaitForReactionAsync(x => x.User.Id == ctx.User.Id &&
                                                                          x.Message.Id == setMessage.Id &&
                                                                          emotes.Contains(x.Emoji), TimeSpan.FromMinutes(5));                                        

                    if (result.TimedOut) break;

                    if (result.Result.Emoji == emotes[0])
                    {
                        curSet--;
                        if (curSet < 0) curSet += sets.Count();                   
                    }
                    else if (result.Result.Emoji == emotes[2])
                    {
                        curSet++;
                        if (curSet >= sets.Count()) curSet -= sets.Count();                
                    }
                    else if (result.Result.Emoji == emotes[3])
                    {
                        break;
                    }
                    else if (result.Result.Emoji == emotes[1])
                    {
                        if (curRarities[curSet] == Rarity.NO_RARITY) curRarities[curSet] = Rarity.Common;
                        else if (curRarities[curSet] == Rarity.Common) curRarities[curSet] = Rarity.Rare;
                        else if (curRarities[curSet] == Rarity.Rare) curRarities[curSet] = Rarity.Epic;
                        else if (curRarities[curSet] == Rarity.Epic) curRarities[curSet] = Rarity.Legendary;
                        else if (curRarities[curSet] == Rarity.Legendary) curRarities[curSet] = Rarity.NO_RARITY;                        
                    }

                    await setMessage.ModifyAsync(embed: GetSetMessageEmbed(sets, curSet, curRarities[curSet]).Build()).ConfigureAwait(false);
                    
                    //await setMessage.DeleteReactionAsync(result.Result.Emoji, ctx.User).ConfigureAwait(false);
                }

                DiscordEmbedBuilder inactiveEmbed = GetSetMessageEmbed(sets, curSet, curRarities[curSet]);
                                                

                inactiveEmbed.Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "Embed Inactive", IconUrl = "https://cdn.discordapp.com/attachments/793064184699027470/834683323632320532/no-entry-sign_1f6ab.png" };
                await setMessage.ModifyAsync(embed: inactiveEmbed.Build()).ConfigureAwait(false);

            }

            [Aliases("set")]
            [Command("sets")]
            public async Task ShowSetInfo(CommandContext ctx, [RemainingText] string setName)
            {
                setName = setName.ToLower();
                setName = SetHandler.SetAttributeToString[SetHandler.LowercaseToSetAttribute[setName]];

                if (!BotHandler.setHandler.Sets.ContainsKey(setName))
                {
                    //package not found
                    await ctx.RespondAsync(embed: new DiscordEmbedBuilder
                    {
                        Title = "No Such Set Found",
                        Description = "Make sure you've typed the name correctly.",
                        Color = DiscordColor.Red
                    }).ConfigureAwait(false);
                }
                else
                {
                    var embed = new DiscordEmbedBuilder()
                    {
                        Title = $"{setName}",
                        Color = DiscordColor.Azure,
                        Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"Total Upgrades: {BotHandler.setHandler.Sets[setName].Count}" }
                    };

                    string description = string.Empty;

                    Rarity lastRarity = Rarity.NO_RARITY;
                    int rarityCount = 0;

                    for (int i = 0; i < BotHandler.setHandler.Sets[setName].Count(); i++)
                    {
                        if (lastRarity != BotHandler.setHandler.Sets[setName][i].rarity)
                        {
                            if (lastRarity != Rarity.NO_RARITY)
                            {
                                embed.AddField($"{lastRarity} ({rarityCount})", description);
                            }
                            description = string.Empty;
                            rarityCount = 0;
                            lastRarity = BotHandler.setHandler.Sets[setName][i].rarity;
                        }
                        rarityCount++;
                        description += $"- {BotHandler.setHandler.Sets[setName][i]}\n";
                    }
                    embed.AddField($"{lastRarity} ({rarityCount})", description);

                    await ctx.RespondAsync(embed: embed.Build()).ConfigureAwait(false);
                }
            }
        
            [Command("test")]
            public async Task EmoteTest(CommandContext ctx)
            {
                int counter = 0;

                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Title = $"{counter}"
                };
                                

                var msg = await ctx.RespondAsync(embed: embed.Build()).ConfigureAwait(false);

                var interactivity = ctx.Client.GetInteractivity();

                interactivity.Client.MessageReactionRemoved += async (client, args) => {                    

                    await ctx.RespondAsync("Frog").ConfigureAwait(false);
                };
            }            
        }               
    }
}
