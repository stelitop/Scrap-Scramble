using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Scrap_Scramble_Final_Version.GameRelated;
using Scrap_Scramble_Final_Version.GameRelated.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.BotRelated
{    
    public class BotHandler
    {
        public static Bot DiscordBot = new Bot();

        public static SetHandler setHandler = new SetHandler();

        public readonly static Dictionary<ulong, UserState> _userState = new Dictionary<ulong, UserState>();        


        public static UserState GetUserState(ulong id)
        {
            if (_userState.ContainsKey(id)) return _userState[id];
            else
            {
                _userState.Add(id, UserState.Idle);
                return UserState.Idle;
            }
        }
        public static void SetUserState(ulong id, UserState state)
        {
            if (_userState.ContainsKey(id)) _userState[id] = state;
            else _userState.Add(id, state);    
        }

        // key = host id
        public static Dictionary<ulong, Room> openRooms = new Dictionary<ulong, Room>();        

        public static class Rooms
        {
            public static Room GetUserRoom(ulong id)
            {
                foreach (var room in BotHandler.openRooms)
                {
                    if (room.Value.players.Contains(id)) return room.Value;
                }
                return null;
            }
            public static Player GetUserPlayer(ulong id)
            {
                var room = BotHandler.Rooms.GetUserRoom(id);
                if (room == null) return null;
                if (!room.gameHandler.players.ContainsKey(id)) return null;

                return room.gameHandler.players[id];
            }
            public static async Task StartRoom(CommandContext ctx, ulong hostId)
            {
                if (openRooms.ContainsKey(hostId))
                {
                    await openRooms[hostId].StartGame(ctx);
                    openRooms.Remove(hostId);
                    //start a game
                }
                else
                {
                    await ctx.RespondAsync(new DiscordEmbedBuilder
                    {
                        Title = "You Aren't Hosting a Room",
                        Description = "This message shouldn't actually appear. Report this as a bug.",
                        Color = DiscordColor.Violet
                    }).ConfigureAwait(false);
                }
            }
            public static bool CreateRoom(CommandContext ctx)
            {
                if (openRooms.ContainsKey(ctx.User.Id)) return false;
                if (BotHandler.GetUserState(ctx.User.Id) != UserState.Idle) return false;

                Room newRoom = new Room();
                newRoom.SetDefaultName(ctx, BotHandler.openRooms);


                openRooms.Add(ctx.User.Id, newRoom);
                openRooms[ctx.User.Id].AddPlayer(ctx.User.Id);
                openRooms[ctx.User.Id].hostId = ctx.User.Id;
                openRooms[ctx.User.Id].guild = ctx.Guild;

                BotHandler.SetUserState(ctx.User.Id, UserState.HostingARoom);

                return true;
            }
            public static async Task DisbandRoom(CommandContext ctx, ulong hostId)
            {
                if (!BotHandler.openRooms.ContainsKey(hostId))
                {
                    await ctx.RespondAsync(new DiscordEmbedBuilder
                    {
                        Title = "You're Not Hosting a Room",
                        Description = "You shouldn't be seeing this message normally. This is a bug.",
                        Color = DiscordColor.Violet
                    }).ConfigureAwait(false);

                    return;
                }

                foreach (var player in BotHandler.openRooms[hostId].players)
                {
                    BotHandler.SetUserState(player, UserState.Idle);                     
                }

                BotHandler.openRooms.Remove(hostId);

                await ctx.RespondAsync(new DiscordEmbedBuilder { 
                    Title = "Room Disbanded Successfully",
                    Color = DiscordColor.Green
                }).ConfigureAwait(false);
            }
            public static bool IsNameAvailable(CommandContext ctx, string roomName)
            {
                foreach (var room in openRooms)
                {
                    if (room.Value.guild.Id == ctx.Guild.Id && room.Value.roomName.Equals(roomName)) return false;
                }
                return true;
            }
        }
    
        public static bool FilterName(ref string name, out string message, int minLen = 3, int maxLen = 20)
        {
            const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890+-=/!#$%^&()? .,\'\"";

            if (name == null)
            {
                message = $"The name cannot be empty";
                return false;
            }

            for (int i=0; i<name.Length; i++)
            {
                if (!AllowedCharacters.Contains(name[i]))
                {
                    message = $"The name contains the illegal character '{name[i]}'";
                    return false;
                }
            }

            name = name.Trim();

            for (int i=1; i<name.Length; i++)
            {
                if (name[i] == name[i-1] && name[i] == ' ')
                {
                    name.Remove(i, 1);
                    i--;
                }
            }

            if (name.Length < minLen)
            {
                message = $"The name needs to be at least {minLen} characters long, yours is {name.Length}.";
                return false;
            }
            if (name.Length > maxLen)
            {
                message = $"The name needs to be maximum {maxLen} characters long, yours is {name.Length}.";
                return false;
            }

            message = "Valid name";
            return true;
        }
    }
}
