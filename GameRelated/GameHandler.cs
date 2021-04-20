using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Scrap_Scramble_Final_Version.BotRelated;
using Scrap_Scramble_Final_Version.GameRelated.Cards;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated
{   
    [Serializable]
    public class GameHandler
    {
        public static Random randomGenerator = new Random();
        //public SetHandler setHandler;
        public CardPool pool;

        public Dictionary<ulong, Player> players;
        public PairsHandler pairsHandler;
        public int curMaxMana = 10;

        //public CombatOutputCollector combatOutputCollector;

        public int currentRound = 1;

        public int amountReady = 0;

        public GameSettings gameSettings;

        public DiscordChannel outputChannel;
        public DiscordMessage playerListMessage;

        public CancellationTokenSource waitingTokenSource;

        public GameHandler()
        {
            this.players = new Dictionary<ulong, Player>();
            this.pool = new CardPool();
            this.pool.FillGenericCardPool();
            //this.setHandler = new SetHandler();
            this.pairsHandler = new PairsHandler();
            //this.combatOutputCollector = new CombatOutputCollector();
            this.gameSettings = new GameSettings();

            this.outputChannel = null;
            this.playerListMessage = null;

            this.waitingTokenSource = null;
        }

        public void AddPlayer(ulong id, string name)
        {
            this.players.Add(id, new Player(name));
            //this.pairsHandler.AddPlayer();

            //do some other stuff later
        }
        public void RemovePlayer(ulong id)
        {
            if (!this.players.ContainsKey(id)) return;

            this.players.Remove(id);

            //do some other stuff later
            //including fixing relative values
        }
        public Task StartNewGame(Room room, CommandContext ctx)
        {           
            this.curMaxMana = this.gameSettings.startingMana;
            this.amountReady = 0;
            
            CardPool pool = new CardPool();
            pool.FillCardPoolWithSets(this.gameSettings.setAmount, BotHandler.setHandler);            
            
            this.players.Clear();
            this.currentRound = 1;
            foreach (var id in room.players)
            {
                if (room.nicknames.ContainsKey(id)) this.players.Add(id, new Player(room.nicknames[id]));
                else this.players.Add(id, new Player());
                
                this.players[id].pool = new CardPool(pool);
                this.players[id].maxMana = this.curMaxMana;                
                this.players[id].shop.Refresh(this, this.players[id].pool, this.players[id].maxMana);                
                this.players[id].curMana = this.curMaxMana;
                this.players[id].lives = this.gameSettings.startingLives;

                this.players[id].ready = false;                
            }
            
            //calls the next pairs method
            this.pairsHandler = new PairsHandler(this);
            this.pairsHandler.NextRoundPairs(this);

            return Task.CompletedTask;

            //do other stuff like matching later
        }

        public async Task NextRound(Room room, CommandContext ctx)
        {
            Console.WriteLine("Frog1");
            this.currentRound++;
            this.pairsHandler.NextRoundPairs(this);
            this.amountReady = 0;
            Console.WriteLine("Frog2");
            int newMana = Math.Max(0, Math.Min(5, this.gameSettings.maxManaCap - this.curMaxMana));

            this.curMaxMana += newMana;
            Console.WriteLine("Frog3");
            foreach (var player in this.players)
            {
                player.Value.aftermathMessages.Clear();

                player.Value.maxMana += newMana;

                player.Value.shop.Refresh(this, player.Value.pool, player.Value.maxMana);

                player.Value.overloaded = player.Value.creatureData.staticKeywords[Cards.StaticKeyword.Overload];
                player.Value.curMana = Math.Max(0, player.Value.maxMana - player.Value.overloaded);

                player.Value.ready = false;

                player.Value.playHistory.Add(new List<Cards.Card>());
                player.Value.buyHistory.Add(new List<Cards.Upgrade>());

                player.Value.creatureData.InitStaticKeywordsDictionary();
            }
            Console.WriteLine("Frog4");
            foreach (var player in this.players)
            {
                ExtraEffectInfo aftermathMeInfo = new ExtraEffectInfo(ctx);
                await Effect.CallEffects(player.Value.effects, Effect.EffectType.AftermathMe, null, this, player.Key, this.pairsHandler.opponents[player.Key], aftermathMeInfo);
            }
            Console.WriteLine("Frog5");
            foreach (var player in this.players)
            {
                ExtraEffectInfo aftermathEnemyInfo = new ExtraEffectInfo(ctx);
                await Effect.CallEffects(player.Value.effects, Effect.EffectType.AftermathEnemy, null, this, player.Key, this.pairsHandler.opponents[player.Key], aftermathEnemyInfo);                            
            }
            Console.WriteLine("Frog6");
            foreach (var player in this.players)
            {
                player.Value.effects = player.Value.nextRoundEffects;
                player.Value.nextRoundEffects = new List<Effect>();
            }
            Console.WriteLine("Frog7");
            foreach (var player in this.players)
            {
                player.Value.attachedUpgrades.Clear();
                player.Value.hand.RemoveAllBlankUpgrades();
                player.Value.specificEffects = new SpecificEffects();
            }
            Console.WriteLine("Frog8");
        }

        public async Task<FightOutputCollector> Fight(ulong mech1, ulong mech2, CommandContext ctx)
        {
            Console.WriteLine("fight1");
            if (!this.players.ContainsKey(mech1) || !this.players.ContainsKey(mech2)) return null;
            Console.WriteLine("fight2");
            FightOutputCollector outputCollector = new FightOutputCollector();
            Console.WriteLine("fight3");
            this.players[mech1].destroyed = false;
            this.players[mech2].destroyed = false;

            int rows1 = 0, rows2 = 0;

            outputCollector.playerInfoOutput1.Add(this.players[mech1].GetUpgradesList(out rows1));
            outputCollector.playerInfoOutput2.Add(this.players[mech2].GetUpgradesList(out rows2));
            Console.WriteLine("fight4");
            for (int i = 0; i < rows2 - rows1; i++)
                outputCollector.playerInfoOutput1.Add(string.Empty);

            for (int i = 0; i < rows1 - rows2; i++)
                outputCollector.playerInfoOutput2.Add(string.Empty);
            Console.WriteLine("fight5");
            CreatureData crData1 = this.players[mech1].creatureData.DeepCopy();
            CreatureData crData2 = this.players[mech2].creatureData.DeepCopy();

            outputCollector.playerInfoOutput1.Add("\n" + this.players[mech1].GetInfoForCombat(this));
            outputCollector.playerInfoOutput2.Add("\n" + this.players[mech2].GetInfoForCombat(this));

            int prStat1 = crData1.staticKeywords[StaticKeyword.Rush] - crData1.staticKeywords[StaticKeyword.Taunt];
            int prStat2 = crData2.staticKeywords[StaticKeyword.Rush] - crData2.staticKeywords[StaticKeyword.Taunt];
            Console.WriteLine("fight6");
            //false = mech1 wins, true = mech2 wins
            bool result;
            //for output purposes

            bool coinflip = false;
            Console.WriteLine("fight7");
            //see who has bigger priority
            if (prStat1 > prStat2) result = false;
            else if (prStat1 < prStat2) result = true;
            //if tied, check the tiebreaker
            else if (crData1.staticKeywords[StaticKeyword.Tiebreaker] > crData2.staticKeywords[StaticKeyword.Tiebreaker]) result = false;
            else if (crData1.staticKeywords[StaticKeyword.Tiebreaker] < crData2.staticKeywords[StaticKeyword.Tiebreaker]) result = true;
            //roll random
            else
            {
                coinflip = true;
                if (GameHandler.randomGenerator.Next(0, 2) == 0) result = false;
                else result = true;
            }
            Console.WriteLine("fight8");
            if (result == true)
            {
                ulong mid = mech1;
                mech1 = mech2;
                mech2 = mid;

                CreatureData midCrData = crData1.DeepCopy();
                crData1 = crData2.DeepCopy();
                crData2 = midCrData.DeepCopy();
            }
            Console.WriteLine("fight9");
            //-preCombat header               

            if (!coinflip)
            {
                outputCollector.preCombatOutput.Add($"{this.players[mech1].name} has Attack Priority.");
                if (this.players[mech1].specificEffects.invertAttackPriority || this.players[mech2].specificEffects.invertAttackPriority)
                {
                    outputCollector.preCombatOutput.Add($"Because of a Trick Roomster, {this.players[mech2].name} has Attack Priority instead.");

                    ulong mid = mech1;
                    mech1 = mech2;
                    mech2 = mid;
                    
                    CreatureData midCrData = crData1.DeepCopy();
                    crData1 = crData2.DeepCopy();
                    crData2 = midCrData.DeepCopy();
                }
            }
            else outputCollector.preCombatOutput.Add($"{this.players[mech1].name} wins the coinflip for Attack Priority.");

            Console.WriteLine("fight10");

            outputCollector.goingFirst = mech1;

            //need to trigger Start of Combat here

            for (int multiplier = 0; multiplier < this.players[mech1].specificEffects.multiplierStartOfCombat; multiplier++)
            {
                ExtraEffectInfo.StartOfCombatInfo SOCInfo = new ExtraEffectInfo.StartOfCombatInfo(ctx, mech1);

                await Effect.CallEffects(this.players[mech1].effects, Effect.EffectType.StartOfCombat, null, this, mech1, mech2, SOCInfo);

                for (int i=0; i<SOCInfo.output.Count(); i++)
                {
                    outputCollector.preCombatOutput.Add(SOCInfo.output[i]);
                }

                //need to do something about effects that trigger whenever a start of combat triggers
            }
            Console.WriteLine("fight11");
            for (int multiplier = 0; multiplier < this.players[mech2].specificEffects.multiplierStartOfCombat; multiplier++)
            {
                ExtraEffectInfo.StartOfCombatInfo SOCInfo = new ExtraEffectInfo.StartOfCombatInfo(ctx, mech2);                

                await Effect.CallEffects(this.players[mech2].effects, Effect.EffectType.StartOfCombat, null, this, mech2, mech1, SOCInfo);

                for (int i = 0; i < SOCInfo.output.Count(); i++)
                {
                    outputCollector.preCombatOutput.Add(SOCInfo.output[i]);
                }

                //need to do something about effects that trigger whenever a start of combat triggers
            }
            Console.WriteLine("fight12");
            //-preCombat header


            //combat header
            for (int curAttacker = 0; this.players[mech1].IsAlive() && this.players[mech2].IsAlive(); curAttacker++)
            {
                Console.WriteLine("attacking1");
                ulong attacker, defender;
                if (curAttacker % 2 == 0)
                {
                    attacker = mech1;
                    defender = mech2;
                }
                else
                {
                    attacker = mech2;
                    defender = mech1;
                }
                Console.WriteLine("attacking2");
                // If needed to change the attack mech method to 
                var damageInfo = await this.players[attacker].AttackMech(this, attacker, defender, ctx);
                Console.WriteLine("attacking3");
                outputCollector.combatOutput.Add(damageInfo.output);
                Console.WriteLine("attacking4");

                if (!this.players[mech1].IsAlive() || !this.players[mech2].IsAlive()) break;
                //should stop when a player is dead
                Console.WriteLine("attacking5");
                var combatInfo = new ExtraEffectInfo.AfterAttackInfo(ctx, damageInfo.dmg);
                Console.WriteLine("attacking6");
                await Effect.CallEffects(this.players[attacker].effects, Effect.EffectType.AfterThisAttacks, null, this, attacker, defender, combatInfo);
                Console.WriteLine("attacking7");
                //should stop when a player is dead
                combatInfo.calledEffect = Effect.EffectType.AfterTheEnemyAttacks;
                await Effect.CallEffects(this.players[attacker].effects, Effect.EffectType.AfterTheEnemyAttacks, null, this, defender, attacker, combatInfo);
                Console.WriteLine("attacking8");
            }
            Console.WriteLine("fight13");

            if (this.players[mech1].IsAlive())
            {
                outputCollector.combatOutput.Add($"{this.players[mech1].name} has won!");

                if (this.players[mech1].specificEffects.invertAttackPriority || this.players[mech2].specificEffects.invertAttackPriority) this.players[mech1].lives++;
                else this.players[mech2].lives--;

                this.pairsHandler.playerResults[this.pairsHandler.playerResults.Count - 1][mech1] = FightResult.WIN;
                this.pairsHandler.playerResults[this.pairsHandler.playerResults.Count - 1][mech2] = FightResult.LOSS;
            }
            else
            {
                outputCollector.combatOutput.Add($"{this.players[mech2].name} has won!");

                if (this.players[mech1].specificEffects.invertAttackPriority || this.players[mech2].specificEffects.invertAttackPriority) this.players[mech2].lives++;
                else this.players[mech1].lives--;

                this.pairsHandler.playerResults[this.pairsHandler.playerResults.Count - 1][mech1] = FightResult.LOSS;
                this.pairsHandler.playerResults[this.pairsHandler.playerResults.Count - 1][mech2] = FightResult.WIN;
            }
            Console.WriteLine("fight14");
            //-combat header

            //revert to before the fight
            this.players[mech1].creatureData = crData1.DeepCopy();
            this.players[mech2].creatureData = crData2.DeepCopy();

            this.players[mech1].destroyed = false;
            this.players[mech2].destroyed = false;
            Console.WriteLine("fight15");
            return outputCollector;
        }

        public int AlivePlayers
        {
            get
            {
                int ret = 0;
                foreach (var player in players)
                {
                    if (player.Value.lives > 0) ret++;
                }
                return ret;
            }
        }

        public async Task SendShopsAsync(CommandContext ctx)
        {
            foreach (var player in this.players)
            {
                await player.Value.SendNewPlayerUI(ctx, this, player.Key);
            }
        }


        private async Task<DiscordEmbedBuilder> GetInteractivePlayerListEmbedAsync(CommandContext ctx)
        {
            string description = string.Empty;

            foreach (var player in this.players)
            {
                if (!description.Equals(string.Empty)) description += '\n';

                if (player.Value.lives < 1) description += ":skull:";
                else if (player.Value.ready) description += ":green_square: ";
                else description += ":red_square: ";

                description += $"{player.Value.name} ({(await ctx.Client.GetUserAsync(player.Key)).Username})";

                if (player.Value.lives >= 1) description += $" - Lives: {player.Value.lives}";
            }

            DiscordEmbedBuilder ret = new DiscordEmbedBuilder { 
                Title = "List of Players",
                Description = description,
                Color = DiscordColor.Azure
            };

            return ret;
        }
        public async Task SendNewInteractivePlayerList(CommandContext ctx)
        {
            if (this.outputChannel == null) return;

            this.playerListMessage = await this.outputChannel.SendMessageAsync((await this.GetInteractivePlayerListEmbedAsync(ctx)).Build()).ConfigureAwait(false);
            
        }
        public async Task RefreshInteractivePlayerList(CommandContext ctx)
        {
            if (this.playerListMessage == null)
            {
                await this.SendNewInteractivePlayerList(ctx);
            }
            else
            {
                await this.playerListMessage.ModifyAsync((await this.GetInteractivePlayerListEmbedAsync(ctx)).Build()).ConfigureAwait(false);
            }
        }
    }
}
