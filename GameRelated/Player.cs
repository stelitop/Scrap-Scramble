using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Scrap_Scramble_Final_Version.GameRelated.Cards;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated
{
    public class Player
    {
        public CreatureData creatureData = new CreatureData();
        public Shop shop = new Shop();
        public Hand hand = new Hand();

        public int curMana = 10;
        public int overloaded = 0;
        public int maxMana = 10;

        public int lives = 0;

        public string name = "Yepge";

        public List<Upgrade> attachedUpgrades = new List<Upgrade>();
        public List<Effect> effects = new List<Effect>();
        public SpecificEffects specificEffects = new SpecificEffects();

        public List<List<Card>> playHistory = new List<List<Card>>();
        public List<List<Upgrade>> buyHistory = new List<List<Upgrade>>();

        public bool destroyed = false;

        public List<string> aftermathMessages = new List<string>();
        public List<Effect> nextRoundEffects = new List<Effect>();

        public CardPool pool = new CardPool();

        public bool ready = false;

        public DiscordMessage interactiveMessage = null;

        public Player() 
        {
            this.playHistory.Add(new List<Card>());
            this.buyHistory.Add(new List<Upgrade>());
        }

        public Player(string name)
        {
            this.name = name;
            this.playHistory.Add(new List<Card>());
            this.buyHistory.Add(new List<Upgrade>());
        }

        public string PrintInfoGeneral(GameHandler gameHandler, ulong curPlayer)
        {
            string ret = string.Empty;

            ret += $"**{this.creatureData.attack}/{this.creatureData.health}**";
            ret += $"\nMana: {this.curMana}/{this.maxMana}";
            if (this.overloaded > 0) ret += $"\n ({this.overloaded} Overloaded)";
            if (this.lives > 1) ret += $"\nLives: {this.lives}";
            else ret += "\nLives: **1** (!)";
            ret += "\nOpponent: ";
            if (gameHandler.pairsHandler.opponents[curPlayer] != curPlayer) ret += $"{gameHandler.players[gameHandler.pairsHandler.opponents[curPlayer]].name}";
            else ret += "None";

            return ret;
        }
        public string PrintInfoKeywords(GameHandler gameHandler)
        {
            string ret = string.Empty;

            this.creatureData.staticKeywords[StaticKeyword.Echo] = 0;
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 0;
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 0;

            foreach (var kw in this.creatureData.staticKeywords)
            {
                if (kw.Key == StaticKeyword.Echo) continue;
                if (kw.Key == StaticKeyword.Binary) continue;
                if (kw.Key == StaticKeyword.Magnetic) continue;

                if (kw.Value != 0)
                {
                    if (ret.Equals(string.Empty)) ret += $"{kw.Key}: {kw.Value}";
                    else ret += $"\n{kw.Key}: {kw.Value}";
                }
            }

            if (ret.Equals(string.Empty)) return "(none)";
            return ret;
        }
        public string PrintInfoEffects(GameHandler gameHandler)
        {
            string ret = string.Empty;
            foreach (var effect in this.effects)
            {
                if (effect.displayMode == Effect.EffectDisplayMode.Public || effect.displayMode == Effect.EffectDisplayMode.Private)
                    ret += $"{effect.effectText}\n";
            }

            if (ret.Equals(string.Empty)) ret = "(none)";

            return ret;
        }
        public string PrintInfoUpgrades(GameHandler gameHandler)
        {
            string ret = this.GetUpgradesList(out int rows, false);

            if (ret.Equals(string.Empty) || rows == 0) return "(none)";
            return ret;
        }
        public string GetAftermathMessages()
        {
            string ret = string.Empty;
            for (int i = 0; i < this.aftermathMessages.Count(); i++) ret = ret + this.aftermathMessages[i] + "\n";
            return ret;
        }

        public DiscordEmbedBuilder GetUIEmbed(GameHandler gameHandler, ulong curPlayer)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"{this.name}'s Information",
                Color = DiscordColor.Brown,
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "Type >help to see what commands are available. Commands related to your mech can only be used in DMs. When you're done with your turn, type >ready." }
            };

            var aftermath = this.GetAftermathMessages();
            if (!aftermath.Equals(string.Empty))
            {
                embed.AddField("[Aftermath]", aftermath);
            }

            embed.AddField("[Mech Info]", this.PrintInfoGeneral(gameHandler, curPlayer), true);
            embed.AddField("[Keyword]", this.PrintInfoKeywords(gameHandler), true);
            embed.AddField("[Upgrades]", this.PrintInfoUpgrades(gameHandler), true);
            embed.AddField("[Effects]", this.PrintInfoEffects(gameHandler));

            var shop = this.shop.GetShopInfo(gameHandler, curPlayer); 
            for (int i = 0; i < shop.Count; i++)
            {
                embed.AddField($"[Round {gameHandler.currentRound} Shop]", shop[i]);
            }

            var hand = this.hand.GetHandInfo(gameHandler, curPlayer);
            for (int i = 0; i < hand.Count; i++)
            {
                embed.AddField($"[Your Hand]", hand[i]);
            }

            return embed;
        }
        public async Task<bool> SendNewPlayerUI(CommandContext ctx, GameHandler gameHandler, ulong curPlayer)
        {
            if (!ctx.Channel.IsPrivate)
            {
                var playerDms = await (await ctx.Guild.GetMemberAsync(curPlayer).ConfigureAwait(false)).CreateDmChannelAsync().ConfigureAwait(false);

                this.interactiveMessage = await playerDms.SendMessageAsync(this.GetUIEmbed(gameHandler, curPlayer).Build()).ConfigureAwait(false);
            }
            else
            {
                this.interactiveMessage = await ctx.RespondAsync(this.GetUIEmbed(gameHandler, curPlayer).Build()).ConfigureAwait(false);
            }

            return true;
        }
        public async Task<bool> RefreshPlayerUI(GameHandler gameHandler, ulong curPlayer)
        {
            if (this.interactiveMessage == null) return false;

            await this.interactiveMessage.ModifyAsync(embed: this.GetUIEmbed(gameHandler, curPlayer).Build()).ConfigureAwait(false);

            return true;
        }

        public async Task AttachMech(Upgrade mech, GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
        {
            ExtraEffectInfo extraInf = new ExtraEffectInfo(ctx);
            await Effect.CallEffects(mech.effects, Effect.EffectType.OnPlay, mech, gameHandler, curPlayer, enemy, extraInf);            

            if (mech.creatureData.staticKeywords[StaticKeyword.Magnetic] > 0)
            {
                for (int i = 0; i < mech.creatureData.staticKeywords[StaticKeyword.Magnetic]; i++)
                {
                    await PlayerInteraction.ActivateMagneticAsync(gameHandler, curPlayer, enemy, ctx);
                }
            }

            if (mech.creatureData.staticKeywords[StaticKeyword.Echo] > 0)
            {
                gameHandler.players[curPlayer].shop.AddUpgrade(mech.BasicCopy(gameHandler.players[curPlayer].pool));
            }

            if (mech.creatureData.staticKeywords[StaticKeyword.Binary] > 0)
            {
                mech.creatureData.staticKeywords[StaticKeyword.Binary]--;

                Upgrade binaryLessCopy = mech.BasicCopy(gameHandler.players[curPlayer].pool);
                binaryLessCopy.cardText += " (No Binary)";
                binaryLessCopy.creatureData.staticKeywords[StaticKeyword.Binary]--;

                //the copy should have basic stats
                gameHandler.players[curPlayer].hand.AddCard(binaryLessCopy);

            }
            mech.creatureData.staticKeywords[StaticKeyword.Binary] = 0;

            this.creatureData += mech.creatureData;

            this.creatureData.staticKeywords[StaticKeyword.Echo] = 0;
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 0;
            this.creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 0;

            //await mech.Battlecry(gameHandler, curPlayer, enemy);
            await Effect.CallEffects(mech.effects, Effect.EffectType.Battlecry, mech, gameHandler, curPlayer, enemy, extraInf);

            if (gameHandler.players[curPlayer].playHistory[gameHandler.players[curPlayer].playHistory.Count() - 1].Count() > 0)
            {
                await Effect.CallEffects(mech.effects, Effect.EffectType.Combo, mech, gameHandler, curPlayer, enemy, extraInf);
            }

            this.attachedUpgrades.Add((Upgrade)mech.DeepCopy());

            foreach (var upgradeEffect in mech.effects)
            {
                this.effects.Add(upgradeEffect.Copy());
            }
        }
        public async Task<bool> BuyCard(int shopPos, GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
        {
            bool result = this.shop.At(shopPos).CanBeBought(shopPos, gameHandler, curPlayer, enemy);
            if (!result) return false;

            Card card = this.shop.At(shopPos).DeepCopy();

            this.shop.RemoveUpgrade(shopPos);

            await ((Upgrade)card).BuyCard(shopPos, gameHandler, curPlayer, enemy, ctx);

            this.playHistory.Last().Add(card.DeepCopy());
            this.buyHistory.Last().Add((Upgrade)card.DeepCopy());

            return result;
        }
        public async Task<bool> PlayCard(int handPos, GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
        {
            bool result = this.hand.At(handPos).CanBePlayed(handPos, gameHandler, curPlayer, enemy);
            if (!result) return false;

            Card card = this.hand.At(handPos).DeepCopy();

            this.hand.RemoveCard(handPos);

            await card.PlayCard(handPos, gameHandler, curPlayer, enemy, ctx);

            this.playHistory[this.playHistory.Count() - 1].Add(card.DeepCopy());

            return result;

            //if (handPos >= this.hand.LastIndex) return false;
            //if (this.hand.At(handPos).name == BlankUpgrade.name) return false;
            //Card card = this.hand.At(handPos).DeepCopy();

            //bool result = this.hand.At(handPos).PlayCard(handPos, gameHandler, curPlayer, enemy);

            //if (result)
            //{
            //    this.playHistory[this.playHistory.Count() - 1].Add(card.DeepCopy());

            //    this.hand.RemoveCard(handPos);
            //}
            //return result;
        }


        public async Task TriggerEndOfTurn(GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
        {
            var cards = this.hand.GetAllCardIndexes();
            foreach (var cardIndex in cards)
            {
                ExtraEffectInfo.CardInHandInfo info = new ExtraEffectInfo.CardInHandInfo(ctx, cardIndex);
                Card card = this.hand.At(cardIndex);
                await Effect.CallEffects(card.effects, Effect.EffectType.EndOfTurnInHand, card, gameHandler, curPlayer, enemy, info);
            }
        }

        public string GetUpgradesList(out int rows, bool respectHidden = true)
        {
            if (respectHidden && this.specificEffects.hideUpgradesInLog)
            {
                rows = 1;
                return "(Hidden)";
            }

            string ret = string.Empty;
            rows = 0;

            for (int i = 0; i < this.playHistory[this.playHistory.Count - 1].Count(); i++)
            {
                if (rows >= 20)
                {
                    ret += $"and {this.playHistory[this.playHistory.Count - 1].Count() - i} more...";
                    rows++;
                    break;
                }

                if (this.playHistory[this.playHistory.Count - 1][i].name == BlankUpgrade.name) continue;
                int mult = 1;
                ret += $"- {this.playHistory[this.playHistory.Count - 1][i].name}";

                for (int j = i + 1; j < this.attachedUpgrades.Count(); j++)
                {
                    if (this.playHistory[this.playHistory.Count - 1][j].name == this.playHistory[this.playHistory.Count - 1][i].name) mult++;
                    else break;
                }

                i += mult - 1;

                rows++;

                if (mult > 1) ret += $" x{mult}";
                if (i != this.playHistory[this.playHistory.Count - 1].Count() - 1) ret += "\n";
            }

            return ret;
        }

        public string GetInfoForCombat(GameHandler gameHandler)
        {
            string ret = string.Empty;

            bool isVanilla = true;

            string preCombatEffects = string.Empty;
            for (int i = 0; i < this.effects.Count(); i++)
            {
                if (this.effects[i].effectText.Equals(string.Empty)) continue;
                if (this.effects[i].displayMode != Effect.EffectDisplayMode.Public) continue;

                if (isVanilla) preCombatEffects = this.effects[i].effectText;
                else preCombatEffects += $"\n{this.effects[i].effectText}";
                isVanilla = false;
            }

            if (isVanilla) 
                foreach (var kw in this.creatureData.staticKeywords)
                {
                    if (kw.Value > 0)
                    {
                        isVanilla = false;
                        break;
                    }
                }

            if (isVanilla)
            {
                ret += $"**{this.name} is a {this.creatureData.attack}/{this.creatureData.health} vanilla.**\n";
            }
            else
            {
                ret += $"**{this.name} is a {this.creatureData.attack}/{this.creatureData.health} with:**\n";
                foreach (var kw in this.creatureData.staticKeywords)
                {
                    if (kw.Value == 0) continue;
                    if (kw.Key == StaticKeyword.Overload) continue;
                    ret += $"{kw.Key}: {kw.Value}\n";
                }
            }

            if (!preCombatEffects.Equals(string.Empty)) ret += preCombatEffects + "\n";

            return ret;
        }

        public bool IsAlive()
        {
            if (this.destroyed) return false;
            if (this.creatureData.health <= 0) return false;
            return true;
        }

        public async Task<ExtraEffectInfo.DamageInfo> AttackMech(GameHandler gameHandler, ulong attacker, ulong defender, CommandContext ctx)
        {
            ExtraEffectInfo.DamageInfo damageInfo = new ExtraEffectInfo.DamageInfo(
                ctx, this.creatureData.attack, $"{this.name} attacks for {this.creatureData.attack} damage, ");

            if (this.creatureData.staticKeywords[StaticKeyword.Spikes] > 0 && !gameHandler.players[defender].specificEffects.ignoreSpikes)
            {
                damageInfo.dmg += this.creatureData.staticKeywords[StaticKeyword.Spikes];
                damageInfo.output += $"increased to {damageInfo.dmg} by Spikes, ";
            }

            if (gameHandler.players[defender].creatureData.staticKeywords[StaticKeyword.Shields] > 0 && !this.specificEffects.ignoreShields)
            {
                damageInfo.dmg -= gameHandler.players[defender].creatureData.staticKeywords[StaticKeyword.Shields];
                if (damageInfo.dmg < 0) damageInfo.dmg = 0;
                gameHandler.players[defender].creatureData.staticKeywords[StaticKeyword.Shields] = 0;

                if (this.creatureData.staticKeywords[StaticKeyword.Spikes] > 0)
                {
                    damageInfo.output = $"{this.name} attacks for {this.creatureData.attack} damage, adjusted to {damageInfo.dmg} by Spikes and Shields, ";
                }
                else
                {
                    damageInfo.output += $"reduced to {damageInfo.dmg} by Shields, ";
                }
            }

            if (!gameHandler.players[defender].specificEffects.ignoreSpikes) this.creatureData.staticKeywords[StaticKeyword.Spikes] = 0;

            await gameHandler.players[defender].TakeDamage(gameHandler, attacker, defender, damageInfo);

            return damageInfo;
        }

        public async Task TakeDamage(GameHandler gameHandler, ulong attacker, ulong defender, ExtraEffectInfo.DamageInfo damageInfo )
        {
            damageInfo.calledEffect = Effect.EffectType.BeforeTakingDamage;
            await Effect.CallEffects(this.effects, Effect.EffectType.BeforeTakingDamage, null, gameHandler, defender, attacker, damageInfo);

            this.creatureData.health -= damageInfo.dmg;

            if (this.creatureData.health > 0)
            {
                damageInfo.output += $"reducing {this.name} to {this.creatureData.health} Health.";
            }
            else
            {
                this.destroyed = true;
                damageInfo.output += $"destroying {this.name}.";
                //gameHandler.combatOutputCollector.combatHeader.Add(msg);
                //return damage;
                return;
            }

            //Console.WriteLine("Called: " + msg);
            //gameHandler.combatOutputCollector.combatHeader.Add(msg);

            if (damageInfo.dmg > 0)
            {
                if (gameHandler.players[attacker].creatureData.staticKeywords[StaticKeyword.Poisonous] > 0)
                {
                    this.destroyed = true;
                    //gameHandler.combatOutputCollector.combatHeader.Add($"{gameHandler.players[attacker].name}'s Poisonous destroys {this.name}.");

                    //return damage;
                    return;
                }

                damageInfo.calledEffect = Effect.EffectType.AfterTheEnemyAttacks;
                await Effect.CallEffects(this.effects, Effect.EffectType.AfterThisTakesDamage, null, gameHandler, defender, attacker, damageInfo);
            }

            //return damage;
            return;
        }

    }
}
