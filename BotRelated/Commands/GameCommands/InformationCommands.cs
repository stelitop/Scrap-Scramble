using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
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
            public async Task ShowListOfSets(CommandContext ctx)
            {
                string msg = string.Empty;

                int i = 1;
                
                foreach (var key in BotHandler.setHandler.Sets.Keys)
                {
                    msg += $"{i}) {key}\n";
                    i++;
                }

                await ctx.RespondAsync(embed: new DiscordEmbedBuilder
                {
                    Title = "List of Sets",
                    Description = msg,
                    Color = DiscordColor.Azure
                }).ConfigureAwait(false);
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
        }        
    }
}
