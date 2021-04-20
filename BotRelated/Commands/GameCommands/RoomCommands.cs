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
    [Group("room")]
    public class RoomCommands : BaseCommandModule
    {
        [Command("create")]
        [RequireGuild]
        [UserState(UserState.Idle)]
        public async Task CreateRoom(CommandContext ctx)
        {
            bool result = BotHandler.Rooms.CreateRoom(ctx);

            if (!result)
            {
                //something went wrong
                await ctx.RespondAsync(new DiscordEmbedBuilder
                {
                    Title = "Unable to Create a Room",
                    Description = "You don't meet some of the conditions required to make a room.",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);
            }
            else
            {
                //room created successfully
                await ctx.RespondAsync(new DiscordEmbedBuilder
                {
                    Title = "Room Created Successfully",
                    Color = DiscordColor.Green
                }).ConfigureAwait(false);

                await ctx.RespondAsync(new DiscordEmbedBuilder {
                    Title = "Name Your Room",
                    Description = "Type the name in the chat",
                    Color = DiscordColor.Azure
                }).ConfigureAwait(false);

                var interactivity = ctx.Client.GetInteractivity();

                while (true)
                {
                    var feedback = await interactivity.WaitForMessageAsync(x => x.Channel.Id == ctx.Channel.Id &&
                                                                 x.Author.Id == ctx.User.Id).ConfigureAwait(false);
                    if (feedback.TimedOut)
                    {
                        //timed out
                        await ctx.RespondAsync(new DiscordEmbedBuilder
                        {
                            Title = "Interaction Timed Out",
                            Description = "A default name has been applied, you can change it using \"room rename\"",
                            Color = DiscordColor.Gold
                        }).ConfigureAwait(false);
                        break;
                    }

                    string filterMsg;
                    string name = feedback.Result.Content;
                    bool res = BotHandler.FilterName(ref name, out filterMsg, 3, 40);

                    if (res)
                    {
                        if (BotHandler.Rooms.IsNameAvailable(ctx, name))
                        {
                            //successfully
                            //await feedback.Result.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);
                            await ctx.RespondAsync(new DiscordEmbedBuilder
                            {
                                Title = "Named Assigned Successfully",
                                Description = $"Your room is now called \"{name}\".",
                                Color = DiscordColor.Green
                            }).ConfigureAwait(false);
                            BotHandler.openRooms[ctx.User.Id].roomName = name;
                            break;
                        }
                        else
                        {
                            //there's already a room with that name
                            await ctx.RespondAsync(new DiscordEmbedBuilder
                            {
                                Title = "Room Name Already Taken",
                                Description = "Please choose another name.",
                                Color = DiscordColor.Red
                            }).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        //await feedback.Result.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
                        await ctx.RespondAsync(new DiscordEmbedBuilder {
                            Title = "Invalid Name",
                            Description = filterMsg,
                            Color = DiscordColor.Red
                        }).ConfigureAwait(false);
                    }
                }

                await MechCustomizationCommands.SetName(ctx);
            }
        }

        [Command("disband")]
        [RequireGuild]
        [UserState(new UserState[] { UserState.HostingARoom, UserState.InGame })]
        public async Task DisbandRoom(CommandContext ctx)
        {
            if (!BotHandler.openRooms.ContainsKey(ctx.User.Id))
            {
                await ctx.RespondAsync(new DiscordEmbedBuilder {
                    Title = "You're Not Hosting a Room",
                    Description = "Only the host of the room can use this command.",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);

                return;
            }
            if (BotHandler.openRooms[ctx.User.Id].guild.Id != ctx.Guild.Id)
            {
                await ctx.RespondAsync(new DiscordEmbedBuilder
                {
                    Title = "Your Room Isn't Tied to This Server",
                    Description = "You can only use the disband command in the same server where you created it.",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);

                return;
            }

            await BotHandler.Rooms.DisbandRoom(ctx, ctx.User.Id);
        }

        [Command("list")]
        [RequireGuild]
        public async Task GetListOfRooms(CommandContext ctx)
        {
            List<ulong> roomsInThisServer = new List<ulong>();

            foreach (var room in BotHandler.openRooms)
            {
                if (room.Value.guild.Id == ctx.Guild.Id)
                {
                    roomsInThisServer.Add(room.Key);
                }
            }

            if (roomsInThisServer.Count() == 0)
            {
                await ctx.RespondAsync(new DiscordEmbedBuilder {
                    Title = "There Are No Rooms In This Server",
                    Description = "You can create a room using \"room create\"",
                    Color = DiscordColor.Azure
                }).ConfigureAwait(false);

                return;
            }

            var interactivity = ctx.Client.GetInteractivity();

            List<DSharpPlus.Interactivity.Page> allMenuPages = new List<DSharpPlus.Interactivity.Page>();

            DSharpPlus.Interactivity.PaginationEmojis paginationEmojis = new DSharpPlus.Interactivity.PaginationEmojis
            {
                SkipLeft = DiscordEmoji.FromName(ctx.Client, ":rewind:"),
                Left = DiscordEmoji.FromName(ctx.Client, ":arrow_left:"),
                Stop = DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:"),
                Right = DiscordEmoji.FromName(ctx.Client, ":arrow_right:"),
                SkipRight = DiscordEmoji.FromName(ctx.Client, ":fast_forward:")
            };

            int roomsPerPage = Math.Min(10, roomsInThisServer.Count());
            int totalPages = roomsInThisServer.Count() / roomsPerPage;
            if (roomsInThisServer.Count() % roomsPerPage != 0) totalPages++;

            for (int page = 1; page <= totalPages; page++)
            {
                string description = $"Page {page}/{totalPages}\n";

                for (int i = (page - 1) * roomsPerPage; i < page * roomsPerPage && i < roomsInThisServer.Count(); i++)
                {
                    description += $"\n{i + 1}) {BotHandler.openRooms[roomsInThisServer[i]].roomName} ({(await ctx.Client.GetUserAsync(roomsInThisServer[i])).Username})";
                }

                DSharpPlus.Interactivity.Page menuPage = new DSharpPlus.Interactivity.Page(embed: new DiscordEmbedBuilder
                {
                    Title = "List of Rooms",
                    Description = description,
                    Color = DiscordColor.Azure,
                    Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"Total Rooms: {roomsInThisServer.Count()}" }
                });

                allMenuPages.Add(menuPage);
            }

            await interactivity.SendPaginatedMessageAsync(ctx.Channel, ctx.User, allMenuPages, paginationEmojis, DSharpPlus.Interactivity.Enums.PaginationBehaviour.WrapAround, timeoutoverride: TimeSpan.FromMinutes(3));
        }

        [Command("rename")]
        [RequireGuild]
        [UserState(UserState.HostingARoom)]
        public async Task RenameRoom(CommandContext ctx, [RemainingText] string roomName)
        {
            string filterMsg;
            bool res = BotHandler.FilterName(ref roomName, out filterMsg, 3, 40);

            if (res)
            {
                if (BotHandler.Rooms.IsNameAvailable(ctx, roomName))
                {
                    await ctx.RespondAsync(new DiscordEmbedBuilder {
                        Title = "Room Renamed Successfully",
                        Color = DiscordColor.Green
                    }).ConfigureAwait(false);

                    BotHandler.openRooms[ctx.User.Id].roomName = roomName;
                }
                else
                {
                    await ctx.RespondAsync(new DiscordEmbedBuilder {
                        Title = "Room Name Already Taken",
                        Color = DiscordColor.Red
                    }).ConfigureAwait(false);
                }
            }
            else
            {
                await ctx.RespondAsync(new DiscordEmbedBuilder
                {
                    Title = "Invalid Name",
                    Description = filterMsg,
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);
            }
        }

        [Command("join")]
        [RequireGuild]
        [UserState(UserState.Idle)]
        public async Task JoinRoom(CommandContext ctx, [RemainingText] string roomName)
        {
            string filterMsg;
            bool res = BotHandler.FilterName(ref roomName, out filterMsg, 3, 40);

            if (res)
                foreach (var room in BotHandler.openRooms)
                {
                    if (room.Value.guild.Id == ctx.Guild.Id && room.Value.roomName.Equals(roomName))
                    {
                        room.Value.AddPlayer(ctx.User.Id);
                        await ctx.RespondAsync(new DiscordEmbedBuilder {
                            Title = "Room Joined Successfully",
                            Color = DiscordColor.Green
                        }).ConfigureAwait(false);

                        await MechCustomizationCommands.SetName(ctx);

                        return;
                    }
                }

            await ctx.RespondAsync(new DiscordEmbedBuilder
            {
                Title = "Could Not Join The Room",
                Color = DiscordColor.Red
            }).ConfigureAwait(false);
        }

        [Command("leave")]
        [RequireGuild]
        [UserState(UserState.WaitingInRoom)]
        public async Task LeaveRoom(CommandContext ctx)
        {
            foreach (var room in BotHandler.openRooms)
            {
                if (room.Value.players.Contains(ctx.User.Id))
                {
                    room.Value.players.Remove(ctx.User.Id);
                    BotHandler.SetUserState(ctx.User.Id, UserState.Idle);

                    await ctx.RespondAsync(new DiscordEmbedBuilder {
                        Title = "Room Left Successfully",
                        Color = DiscordColor.Green
                    }).ConfigureAwait(false);

                    return;
                }
            }

            await ctx.RespondAsync(new DiscordEmbedBuilder
            {
                Title = "You Are Not In Any Rooms",
                Description = "You shouln't be seeing this message. This is a bug.",
                Color = DiscordColor.Violet
            }).ConfigureAwait(false);
        }

        [Command("info")]
        [RequireGuild]
        [UserState(new UserState[] { UserState.HostingARoom, UserState.WaitingInRoom, UserState.InGame })]
        public async Task GetRoomInfo(CommandContext ctx)
        {
            Room currentRoom = null;

            foreach (var room in BotHandler.openRooms)
            {
                if (room.Value.players.Contains(ctx.User.Id))
                {
                    if (room.Value.players.Count() == 0) return;
                    currentRoom = room.Value;
                    break;
                }
            }

            if (currentRoom == null) return;

            DiscordEmbedBuilder roomInfo = new DiscordEmbedBuilder {
                Title = $"{currentRoom.roomName} - Room Info",
                Color = DiscordColor.Azure
            };

            string generalInformationField = string.Empty;
            generalInformationField += $"Room Host: {(await ctx.Client.GetUserAsync(currentRoom.hostId)).Username}\n";
            generalInformationField += $"Players in the Room: {currentRoom.players.Count()}\n";
            if (currentRoom.gameHandler.outputChannel == null || !ctx.Guild.Channels.ContainsKey(currentRoom.gameHandler.outputChannel.Id))
            {
                generalInformationField += "Output Channel: None (use \"room bind\")";
            }
            else
            {
                generalInformationField += $"Output Channel: {currentRoom.gameHandler.outputChannel.Mention}";
            }

            roomInfo.AddField("General Information", generalInformationField);

            string playerListField = string.Empty;

            for (int i = 0; i < currentRoom.players.Count(); i++)
            {
                playerListField += $"{currentRoom.nicknames[currentRoom.players[i]]} ({(await ctx.Client.GetUserAsync(currentRoom.players[i]).ConfigureAwait(false)).Username})";
                if (i != currentRoom.players.Count() - 1) playerListField += '\n';
            }
            roomInfo.AddField("Players", playerListField, true);


            roomInfo.AddField("Game Settings",
                $"Starting Lives: {currentRoom.gameHandler.gameSettings.startingLives}\n" +
                $"Starting Mana: {currentRoom.gameHandler.gameSettings.startingMana}\n" +
                $"Maximum Mana: {currentRoom.gameHandler.gameSettings.maxManaCap}\n" +
                $"Amount of Sets: {currentRoom.gameHandler.gameSettings.setAmount}", true);

            await ctx.RespondAsync(roomInfo.Build()).ConfigureAwait(false);
        }

        [Command("bind")]
        [RequireGuild]
        [UserState(UserState.HostingARoom)]
        public async Task BindRoom(CommandContext ctx, DiscordChannel channel)
        {
            if (channel.Guild.Id == BotHandler.openRooms[ctx.User.Id].guild.Id)
            {
                BotHandler.openRooms[ctx.User.Id].gameHandler.outputChannel = channel;

                await ctx.RespondAsync(new DiscordEmbedBuilder {
                    Title = "Room Successfully Bound",
                    Description = $"This room is now bound to {channel.Mention}.",
                    Color = DiscordColor.Green
                }).ConfigureAwait(false);
            }
        }

        [Command("start")]
        [RequireGuild]
        [UserState(UserState.HostingARoom)]
        public async Task StartRoom(CommandContext ctx)
        {
            if (!BotHandler.openRooms.ContainsKey(ctx.User.Id))
            {
                await ctx.RespondAsync(new DiscordEmbedBuilder {
                    Title = "You're Not Hosting a Room",
                    Description = "You shouldn't be seeing this message. This is a bug.",
                    Color = DiscordColor.Violet
                }).ConfigureAwait(false);
                return;
            }

            if (BotHandler.openRooms[ctx.User.Id].roomState != RoomState.WaitingForPlayers)
            {
                switch (BotHandler.openRooms[ctx.User.Id].roomState)
                {
                    case RoomState.InGame:
                        await ctx.RespondAsync(new DiscordEmbedBuilder {
                            Title = "The Room Has Already Started",
                            Color = DiscordColor.Red
                        }).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }

                return;
            };

            //calls the Command method that starts an automatic game
            await AutomaticGame(ctx, BotHandler.openRooms[ctx.User.Id]);
        }

        [Command("nextround")]
        [RequireGuild]
        [UserState(UserState.InGame)]
        public async Task NextRound(CommandContext ctx)
        {
            Room room = BotHandler.Rooms.GetUserRoom(ctx.User.Id);

            await room.gameHandler.outputChannel.SendMessageAsync(new DiscordEmbedBuilder {
                Title = "Next Round Started",
                Description = "Shops will be sent out to all players soon.",
                Color = DiscordColor.Azure
            }).ConfigureAwait(false);

            Console.WriteLine("nextround1");
            await room.NextRound(ctx);
            Console.WriteLine("nextround2");

            await Task.Delay(1000);

            await PairsList(ctx);
        }

        [Command("fight")]
        [RequireGuild]
        [UserState(UserState.InGame)]
        public async Task Fight(CommandContext ctx, ulong pl1, ulong pl2)
        {
            Room room = BotHandler.Rooms.GetUserRoom(ctx.User.Id);
            if (!room.players.Contains(pl1)) return;
            if (!room.players.Contains(pl2)) return;
            var outputMessage = await room.gameHandler.Fight(pl1, pl2, ctx);

            var fightMessage = new DiscordEmbedBuilder
            {
                Title = $"Combat! {room.gameHandler.players[pl1].name} vs {room.gameHandler.players[pl2].name}",
                Color = DiscordColor.Gold
            };

            string msg = string.Empty;
            for (int i = 0; i < outputMessage.playerInfoOutput1.Count(); i++)
            {
                msg = msg + outputMessage.playerInfoOutput1[i] + "\n";
            }
            fightMessage.AddField($"{room.gameHandler.players[pl1].name} upgraded with:", msg, true);

            msg = string.Empty;
            for (int i = 0; i < outputMessage.playerInfoOutput2.Count(); i++)
            {
                msg = msg + outputMessage.playerInfoOutput2[i] + "\n";
            }
            fightMessage.AddField($"{room.gameHandler.players[pl2].name} upgraded with:", msg, true);

            msg = string.Empty;
            for (int i = 0; i < outputMessage.preCombatOutput.Count(); i++)
            {
                msg = msg + outputMessage.preCombatOutput[i] + "\n";
            }
            fightMessage.AddField("[Pre-Combat]", msg);

            msg = string.Empty;
            for (int i = 0; i < outputMessage.combatOutput.Count(); i++)
            {
                msg = msg + outputMessage.combatOutput[i] + "\n";
            }
            fightMessage.AddField("[Combat]", msg);

            await room.gameHandler.outputChannel.SendMessageAsync(fightMessage.Build()).ConfigureAwait(false);
        }

        [Command("dofights")]
        [RequireGuild]
        [UserState(UserState.InGame)]
        public async Task DoAllFights(CommandContext ctx)
        {            
            Room room = BotHandler.Rooms.GetUserRoom(ctx.User.Id);
            if (room == null)
            {
                await ctx.RespondAsync(new DiscordEmbedBuilder{
                    Title = "You're Not In A Room",
                    Description = "You shouldn't be seeing this message, report it as a bug",
                    Color = DiscordColor.Violet
                }).ConfigureAwait(false);
                return;
            }

            if (room.hostId != ctx.User.Id)
            {
                await ctx.RespondAsync(new DiscordEmbedBuilder
                {
                    Title = "You're Not The Room Host",
                    Description = "Only the room host can call this commands.",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);
                return;
            }            

            for (int i = 0; i < room.players.Count(); i++)
            {
                if (room.gameHandler.players[room.players[i]].lives <= 0) continue;
                if (room.players[i] > room.gameHandler.pairsHandler.opponents[room.players[i]]) continue;

                if (room.players[i] < room.gameHandler.pairsHandler.opponents[room.players[i]])
                {
                    await Fight(ctx, room.players[i], room.gameHandler.pairsHandler.opponents[room.players[i]]);

                    await Task.Delay(40000);
                }
            }

            for (int i = 0; i < room.players.Count(); i++)
            {
                if (room.gameHandler.players[room.players[i]].lives <= 0) continue;

                if (room.players[i] == room.gameHandler.pairsHandler.opponents[room.players[i]])
                {
                    await PlayerInfo(ctx, room.players[i]);

                    await Task.Delay(40000);
                }
            }
        }

        private async Task WaitToProceedToFights(Room room)
        {
            room.gameHandler.waitingTokenSource = new CancellationTokenSource();


            Task waitForAllReady = new Task<bool>(() =>
            {
                while (room.gameHandler.amountReady < room.gameHandler.AlivePlayers) ;
                
                return true;
            });

            waitForAllReady.Start();

            await Task.WhenAny(Task.Delay(TimeSpan.FromMinutes(10)), waitForAllReady);
        }

        public async Task AutomaticGame(CommandContext ctx, Room room)
        {
            //calls the Room command that starts a game / tries to start the Room
            bool successfulStart = await room.StartGame(ctx);

            if (!successfulStart)
            {
                await ctx.RespondAsync(new DiscordEmbedBuilder
                {
                    Title = "Something Went Wrong",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);
                return;
            }

            await ctx.RespondAsync(new DiscordEmbedBuilder
            {
                Title = "Game Started Successfully",
                Color = DiscordColor.Green
            }).ConfigureAwait(false);

            do
            {
                //send shops
                await room.gameHandler.SendShopsAsync(ctx).ConfigureAwait(false);
                //show pairs 
                await PairsList(ctx);
                //make a msg that updates on ready
                await room.gameHandler.SendNewInteractivePlayerList(ctx);

                //wait for all players to ready
                await WaitToProceedToFights(room);

                //do end of turn
                foreach (var player in room.gameHandler.players)
                {
                    await player.Value.TriggerEndOfTurn(room.gameHandler, player.Key, room.gameHandler.pairsHandler.opponents[player.Key], ctx);
                }

                //do the fights
                await DoAllFights(ctx);

                if (room.gameHandler.AlivePlayers > 1)
                {
                    await NextRound(ctx);
                }

            } while (room.gameHandler.AlivePlayers > 1);

            ulong winner = 0;
            
            foreach (var player in room.gameHandler.players)
            {
                if (player.Value.lives > 0)
                {
                    winner = player.Key;
                    break;
                }
            }

            if (winner == 0)
            {
                return;
            }

            await room.gameHandler.outputChannel.SendMessageAsync(new DiscordEmbedBuilder { 
                Title = $"{room.nicknames[winner]} Is The Winner!",
                Color = DiscordColor.Gold
            }).ConfigureAwait(false);

            foreach (var player in room.gameHandler.players)
            {
                BotHandler.SetUserState(player.Key, UserState.WaitingInRoom);

                if (room.hostId == player.Key) BotHandler.SetUserState(player.Key, UserState.HostingARoom);
            }
        }

        public async Task PlayerInfo(CommandContext ctx, ulong player)
        {
            Room room = BotHandler.Rooms.GetUserRoom(player);

            if (room == null) return;

            if (room.gameHandler.players[player].lives <= 0)
            {
                //dead player
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder
                {
                    Title = "This Player is Dead",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);
                return;
            }
            

            int dummyInt;
            string desc = string.Empty;


            desc += $"**{room.gameHandler.players[player].name} upgraded with:**\n";
            desc += room.gameHandler.players[player].GetUpgradesList(out dummyInt);
            desc += "\n\n";
            desc += room.gameHandler.players[player].GetInfoForCombat(room.gameHandler);


            await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Title = $"{room.gameHandler.players[player].name}'s Info",
                Description = desc,
                Color = DiscordColor.Gold
            }).ConfigureAwait(false);
            
        }

        //[Aliases("pairlist")]
        //[Command("pairslist")]
        //[RequireGuild]
        [UserState(UserState.InGame)]
        public async Task PairsList(CommandContext ctx)
        {
            string msg = string.Empty;

            GameHandler gameHandler = BotHandler.Rooms.GetUserRoom(ctx.User.Id).gameHandler;

            List<ulong> playerIds = gameHandler.pairsHandler.opponents.Keys.ToList();

            for (int i = 0; i < playerIds.Count(); i++)
            {
                if (!gameHandler.players[playerIds[i]].IsAlive()) continue;

                ulong curPlayer = playerIds[i];
                ulong enemy = gameHandler.pairsHandler.opponents[curPlayer];

                if (curPlayer < enemy) 
                    msg += $"{gameHandler.players[curPlayer].name} vs {gameHandler.players[enemy].name}\n";
            }
            for (int i = 0; i < gameHandler.pairsHandler.opponents.Count(); i++)
            {
                if (gameHandler.players[playerIds[i]].lives <= 0) continue;
                if (playerIds[i] == gameHandler.pairsHandler.opponents[playerIds[i]]) msg += $" {gameHandler.players[playerIds[i]].name} gets a bye\n";
            }

            if (msg.Equals(string.Empty)) msg = "No pairs have been assigned yet.";
            else msg.Trim();

            var responseMessage = new DiscordEmbedBuilder
            {
                Title = "List of Mech Pairs for Combat",
                Description = msg,
                Color = DiscordColor.Azure
            };

            await gameHandler.outputChannel.SendMessageAsync(embed: responseMessage).ConfigureAwait(false);
            //await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }
    }
}
