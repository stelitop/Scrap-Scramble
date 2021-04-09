using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Scrap_Scramble_Final_Version.GameRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.BotRelated.Commands.GameCommands
{
    [Group("mech")]
    public class MechCustomizationCommands : BaseCommandModule
    {
        [UserState(new UserState[] { UserState.WaitingInRoom, UserState.HostingARoom })]
        [Command("rename")]
        public async Task SetNameCommand(CommandContext ctx) => await SetName(ctx);

        public static async Task SetName(CommandContext ctx)
        {
            await ctx.RespondAsync(new DiscordEmbedBuilder{
                Title = "Choose A Name For Your Mech",
                Color = DiscordColor.Azure
            }).ConfigureAwait(false);

            var interactivity = ctx.Client.GetInteractivity();

            Room room = BotHandler.Rooms.GetUserRoom(ctx.User.Id);

            while (true)
            {
                var input = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel.Id == ctx.Channel.Id).ConfigureAwait(false);

                if (input.TimedOut)
                {
                    await ctx.RespondAsync(new DiscordEmbedBuilder
                    {
                        Title = "Interaction Timed Out",
                        Description = "A default name has been applied, you can change it using \"mech rename\"",
                        Color = DiscordColor.Gold
                    }).ConfigureAwait(false);

                    //asign default name

                    string defaultName = ctx.User.Username;
                    bool takenName = false;

                    foreach (var player in room.nicknames)
                    {
                        if (player.Value.Equals(defaultName))
                        {
                            //not good
                            takenName = true;
                            break;
                        }
                    }

                    if (!takenName)
                    {
                        room.nicknames[ctx.User.Id] = defaultName;
                        return;
                    }

                    int extra = 1;
                    while (true)
                    {
                        takenName = false;
                        foreach (var player in room.nicknames)
                        {
                            if (player.Value.Equals($"{defaultName}{extra}"))
                            {
                                extra++;
                                takenName = true;
                                break;
                            }
                        }

                        if (!takenName)
                        {
                            room.nicknames[ctx.User.Id] = $"{defaultName}{extra}";
                            return;
                        }
                    }
                }
                else
                {
                    //check for valid name and not an already used name
                    string filterMsg;
                    string name = input.Result.Content;
                    bool res = BotHandler.FilterName(ref name, out filterMsg, 3, 20);

                    if (res)
                    {
                        bool takenName = false;

                        foreach (var player in room.gameHandler.players)
                        {
                            if (player.Value.name == name)
                            {
                                //not good
                                takenName = true;
                                break;
                            }
                        }

                        if (!takenName)
                        {
                            await ctx.RespondAsync(new DiscordEmbedBuilder
                            {
                                Title = "Named Assigned Successfully",
                                Description = $"Your Mech is now called \"{name}\".",
                                Color = DiscordColor.Green
                            }).ConfigureAwait(false);

                            room.nicknames[ctx.User.Id] = name;
                            return;
                        }
                        else
                        {
                            await ctx.RespondAsync(new DiscordEmbedBuilder
                            {
                                Title = "Mech Name Already Taken",
                                Description = "Please choose another name.",
                                Color = DiscordColor.Red
                            }).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        //do something
                        await ctx.RespondAsync(new DiscordEmbedBuilder
                        {
                            Title = "Invalid Name",
                            Description = filterMsg,
                            Color = DiscordColor.Red
                        }).ConfigureAwait(false);
                    }
                }

            }
        }
        
    }
}
