using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Scrap_Scramble_Final_Version.GameRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.BotRelated
{
    public enum RoomState
    {
        WaitingForPlayers,
        InGame
    }

    public class Room
    {
        public ulong hostId;
        public DiscordGuild guild;

        public List<ulong> players;
        public Dictionary<ulong, string> nicknames;
        public GameHandler gameHandler;

        public string roomName;

        public RoomState roomState;

        public Room()
        {
            this.players = new List<ulong>();
            this.nicknames = new Dictionary<ulong, string>();
            this.gameHandler = new GameHandler();
            this.hostId = 0;
            this.guild = null;
            this.roomState = RoomState.WaitingForPlayers;
            this.roomName = string.Empty;                
        }
        public void SetDefaultName(CommandContext ctx, Dictionary<ulong, Room> openRooms)
        {
            List<string> roomNames = new List<string>();

            foreach (var room in openRooms)
            {
                if (room.Value.guild.Id == ctx.Guild.Id || room.Value.roomName.StartsWith("NewRoom"))
                {
                    roomNames.Add(room.Value.roomName);
                }
            }

            int index = 1;
            while (roomName.Contains($"NewRoom{index}")) index++;

            this.roomName = $"NewRoom{index}";
        }
        
        public bool AddPlayer(ulong id)
        {
            if (this.players.Contains(id))
            {
                return false;
            }

            this.players.Add(id);
            if (BotHandler.GetUserState(id) != UserState.HostingARoom)
                BotHandler.SetUserState(id, UserState.WaitingInRoom);
            return true;
        }

        public async Task<bool> StartGame(CommandContext ctx)
        {
            if (this.gameHandler.outputChannel == null || !ctx.Guild.Channels.ContainsKey(this.gameHandler.outputChannel.Id))
            {
                await ctx.RespondAsync(new DiscordEmbedBuilder {
                    Title = "There's No Output Channel",
                    Description = "You need to bind the room to a channel for output, use the command \"room bind\".",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);

                return false;
            }

            this.roomState = RoomState.InGame;
            foreach (var id in this.players)
            {
                BotHandler.SetUserState(id, UserState.InGame);
            }            

            await this.gameHandler.StartNewGame(this, ctx);

            return true;
        }

        public async Task<bool> NextRound(CommandContext ctx)
        {
            Console.WriteLine("roomnextround1");
            await this.gameHandler.NextRound(this, ctx);
            Console.WriteLine("roomnextround2");
            foreach (var player in this.gameHandler.players)
            {
                await player.Value.SendNewPlayerUI(ctx, this.gameHandler, player.Key);
            }
            Console.WriteLine("roomnextround3");

            return true;
        }
    }
}
