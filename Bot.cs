using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scrap_Scramble_Final_Version.BotRelated;
using Scrap_Scramble_Final_Version.BotRelated.Commands;
using Scrap_Scramble_Final_Version.BotRelated.Commands.GameCommands;
using ScrapScramble;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrap_Scramble_Final_Version
{
    public class Bot
    {
        public static Bot DiscordBot = new Bot();

        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async void RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,                
                //UseInternalLoggingHandler = true
            };            

            Client = new DiscordClient(config);

            //listens to events
            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(60)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<FunCommands>();
            Commands.RegisterCommands<RoomCommands>();
            Commands.RegisterCommands<PlayerOnlyCommands>();
            Commands.RegisterCommands<MechCustomizationCommands>();
            Commands.RegisterCommands<InformationCommands>();
            //Commands.RegisterCommands<GameCommands>();
            //Commands.RegisterCommands<GameOperatorCommands>();
            //Commands.RegisterCommands<GameSettingsCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task OnClientReady(DiscordClient client, ReadyEventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //load the basic card pool for information purposes
            BotHandler.generalCardPool.FillGenericCardPool();

            //this.LoadKeywordsFromWebsite();

            //await Client.UpdateStatusAsync(new DiscordActivity
            //{
            //    Name = $"({BotInfoHandler.participantsDiscordIds.Count()}) Waiting to >signup",
            //    ActivityType = ActivityType.Playing
            //});

            return;
        }        
    }

    public delegate void VoidNoParamsCaller();
}
