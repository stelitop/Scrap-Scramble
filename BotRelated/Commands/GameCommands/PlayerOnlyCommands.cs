using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Scrap_Scramble_Final_Version.GameRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.BotRelated.Commands.GameCommands
{    
    [UserState(UserState.InGame)]
    [RequireDMs]
    public class PlayerOnlyCommands : BaseCommandModule
    {
        [Command("refreshui")]
        [Description("Sends a new UI that displays your Upgrade's information and deletes the old one (if possible).")]
        public async Task RefreshUserUI(CommandContext ctx)
        {
            var room = BotHandler.Rooms.GetUserRoom(ctx.User.Id);
            await room.gameHandler.players[ctx.User.Id].SendNewPlayerUI(ctx, room.gameHandler, ctx.User.Id);
        }

        [Command("buy")]
        [Description("Buys an Upgrade in your shop and attaches it to your Upgrade.")]
        public async Task BuyUpgrade(CommandContext ctx, [Description("Index of the Upgrade in your shop")] int shopPos)
        {
            shopPos--;
            Room room = BotHandler.Rooms.GetUserRoom(ctx.User.Id);
            Player player = BotHandler.Rooms.GetUserPlayer(ctx.User.Id);

            if (shopPos >= player.shop.LastIndex || shopPos < 0)
            {
                //invalid shop position
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else if (!(await player.BuyCard(shopPos, room.gameHandler, ctx.User.Id, room.gameHandler.pairsHandler.opponents[ctx.User.Id], ctx)))
            {
                //upgrade is too expensive
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else
            {
                //valid pos
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);

                if (player.ready)
                {
                    player.ready = false;
                    room.gameHandler.amountReady--;
                }
                //BotInfoHandler.RefreshPlayerList(ctx);

                await player.RefreshPlayerUI(room.gameHandler, ctx.User.Id);
            }
        }

        [Command("play")]
        [Description("Plays an Upgrade from your hand and attaches it to your Upgrade.")]
        public async Task PlayCard(CommandContext ctx, [Description("Index of the Upgrade in your hand")] int handPos)
        {
            handPos--;

            Room room = BotHandler.Rooms.GetUserRoom(ctx.User.Id);
            Player player = BotHandler.Rooms.GetUserPlayer(ctx.User.Id);                        

            if (handPos >= player.hand.LastIndex || handPos < 0)
            {
                //invalid hand position
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else if (!(await player.PlayCard(handPos, room.gameHandler, ctx.User.Id, room.gameHandler.pairsHandler.opponents[ctx.User.Id], ctx)))
            {
                //upgrade is too expensive
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else
            {
                //valid pos
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);

                if (player.ready)
                {
                    player.ready = false;
                    room.gameHandler.amountReady--;
                }
                //BotInfoHandler.RefreshPlayerList(ctx);

                await player.RefreshPlayerUI(room.gameHandler, ctx.User.Id);
            }
        }

        [Command("ready")]
        [Description("Indicates that you're ready with your play this round.")]
        public async Task ReadyRound(CommandContext ctx)
        {
            Room room = BotHandler.Rooms.GetUserRoom(ctx.User.Id);
            Player player = BotHandler.Rooms.GetUserPlayer(ctx.User.Id);

            DiscordEmbedBuilder responseMessage;

            if (player.ready)
            {
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You Have Already Readied",
                    Color = DiscordColor.Red
                };
            }
            else
            {
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You Have Readied Successfully",
                    Description = "You can still make changes to your Upgrade but you will need to use >ready again.",
                    Color = DiscordColor.Green
                };
                player.ready = true;
                room.gameHandler.amountReady++;

                //int totalReady = 0;
                //foreach (var _player in room.gameHandler.players)
                //{
                //    if (_player.Value.ready) totalReady++;
                //}

                //if (totalReady == room.players.Count)
                //{
                //    await Task.Delay(1000);

                //    await room.gameHandler.outputChannel.SendMessageAsync(new DiscordEmbedBuilder { 
                //        Title = "All Player Have Readied!",
                //        Description = "Use \"room dofights\" to start all fights between the players.",
                //        Color = DiscordColor.Gold
                //    }).ConfigureAwait(false);
                //}

                await room.gameHandler.RefreshInteractivePlayerList(ctx);
            }

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }

        [Command("refresh")]
        [Description("Debug only command. Refreshes the player's shop with a new shop.")]
        public async Task RefreshShopDebug(CommandContext ctx)
        {

            Room room = BotHandler.Rooms.GetUserRoom(ctx.User.Id);
            Player player = BotHandler.Rooms.GetUserPlayer(ctx.User.Id);

            player.shop.Refresh(room.gameHandler, player.pool, player.maxMana);

            await RefreshUserUI(ctx);
        }
    }
}