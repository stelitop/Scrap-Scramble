using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets
{
    public class TinyInventions
    {
        [Upgrade]
        public class RefurbishedPlating : Upgrade
        {
            public RefurbishedPlating() :
                base(UpgradeSet.TinyInventions, "Refurbished Plating", 2, 0, 3, Rarity.Common, "Refurbished Plating - Common - 2/0/2 - Battlecry: Gain +3 Shields for each Taunt you have.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]
                        += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Taunt] * 3;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class Microchip : Upgrade
        {
            public Microchip() :
                base(UpgradeSet.TinyInventions, "Microchip", 0, 1, 1, Rarity.Common, "Overload: (2)")
            {
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 2;
            }
        }

        [Upgrade]
        public class MetallicJar : Upgrade
        {
            public MetallicJar() :
                base(UpgradeSet.TinyInventions, "Metallic Jar", 1, 1, 1, Rarity.Common, "Battlecry: Discover a 1-Cost Upgrade.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }
                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, extraInf.ctx, "Discover a 1-Cost Upgrade",
                        CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.Cost == 1));
                }
            }
        }
    
        [Upgrade]
        public class Healthbox : Upgrade
        {
            public Healthbox() : 
                base(UpgradeSet.TinyInventions, "Healthbox", 2, 0, 8, Rarity.Common, "Start of Combat: Give the enemy Mech +8 Health.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Give the enemy Mech +8 Health.", EffectDisplayMode.Public) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[enemy].creatureData.health += 8;

                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                    info.output.Add(
                        $"{gameHandler.players[curPlayer].name}'s Healthbox gives {gameHandler.players[enemy].name} +8 Health, leaving it with as a {gameHandler.players[enemy].creatureData.Stats()}.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class CutleryDispencer : Upgrade
        {
            public CutleryDispencer() :
                base(UpgradeSet.TinyInventions, "Cutlery Dispencer", 3, 3, 2, Rarity.Common, "Start of Combat: If your Mech has Attack Priority, gain +4 Spikes.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: If your Mech has Attack Priority, gain +4 Spikes.", EffectDisplayMode.Public) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;
                    if (info.firstPlayer == curPlayer)
                    {
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Cutlery Dispencer triggers, giving it +4 Spikes, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes]} Spikes.");
                    }
                    else
                    {
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Cutlery Dispencer fails to trigger.");
                    }

                    return Task.CompletedTask;
                }
            }
        }
    
        [Upgrade]
        public class BootPolisher : Upgrade
        {
            public BootPolisher() :
                base(UpgradeSet.TinyInventions, "Boot Polisher", 3, 2, 3, Rarity.Common, "Start of Combat: If the enemy Mech has Attack Priority, gain +4 Shields.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base() { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;
                    if (info.firstPlayer == enemy)
                    {
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Boot Polisher triggers, giving it +4 Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
                    }
                    else
                    {
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Boot Polisher fails to trigger.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class GoldBolts : Upgrade
        {
            public GoldBolts() :
                base(UpgradeSet.TinyInventions, "Gold Bolts", 3, 3, 2, Rarity.Rare, "Battlecry: Transform your Mech's Shields into Health.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields];
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] = 0;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class WupallSmasher : Upgrade
        {
            public WupallSmasher() :
                base(UpgradeSet.TinyInventions, "Wupall Smasher", 5, 4, 5, Rarity.Rare, "Battlecry: Transform your Mech's Spikes into Attack.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes];
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] = 0;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class BrawlersPlating : Upgrade
        {
            public BrawlersPlating() :
                base(UpgradeSet.TinyInventions, "Brawler's Plating", 2, 1, 2, Rarity.Rare, "After both Mechs have attacked, gain +8 Shields.")
            {
                this.effects.Add(new AttackEffect());
            }
            private class AttackEffect : Effect
            {
                private bool myAttack = false, enemyAttack = false;
                public AttackEffect() : base(new EffectType[]{ EffectType.AfterThisAttacks, EffectType.AfterTheEnemyAttacks}, "After both Mechs have attacked, gain +8 Shields.", EffectDisplayMode.Public) { } 
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (extraInf.calledEffect == EffectType.AfterThisAttacks)
                    {
                        if (enemyAttack && !myAttack)
                        {
                            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;

                            var info = extraInf as ExtraEffectInfo.AfterAttackInfo;
                            info.output.Add(
                                $"{gameHandler.players[curPlayer].name}'s Brawler's Plating triggers, giving it +8 Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]}.");
                        }
                        myAttack = true;
                    }
                    else if (extraInf.calledEffect == EffectType.AfterTheEnemyAttacks)
                    {
                        if (myAttack && !enemyAttack)
                        {
                            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;

                            var info = extraInf as ExtraEffectInfo.AfterAttackInfo;
                            info.output.Add(
                                $"{gameHandler.players[curPlayer].name}'s Brawler's Plating triggers, giving it +8 Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]}.");
                        }
                        enemyAttack = true;
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class PowerGlove : Upgrade
        {
            public PowerGlove() :
                base(UpgradeSet.TinyInventions, "Power Glove", 3, 2, 3, Rarity.Rare, "After both Mechs have attacked, deal 4 damage to the enemy Mech.")
            {
                this.effects.Add(new AttackEffect());
            }
            private class AttackEffect : Effect
            {
                private bool myAttack = false, enemyAttack = false;
                public AttackEffect() : base(new EffectType[] { EffectType.AfterThisAttacks, EffectType.AfterTheEnemyAttacks }, "After both Mechs have attacked, deal 4 damage to the enemy Mech.", EffectDisplayMode.Public) { }
                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (extraInf.calledEffect == EffectType.AfterThisAttacks)
                    {
                        if (enemyAttack && !myAttack)
                        {
                            var attackInfo = new ExtraEffectInfo.DamageInfo(extraInf.ctx, 4, 
                                $"{gameHandler.players[curPlayer].name}'s Power Glove triggers, dealing 4 damage, ");

                            await gameHandler.players[enemy].TakeDamage(gameHandler, curPlayer, enemy, attackInfo);

                            var info = extraInf as ExtraEffectInfo.AfterAttackInfo;
                            info.output.Add(attackInfo.output);                                
                        }
                        myAttack = true;
                    }
                    else if (extraInf.calledEffect == EffectType.AfterTheEnemyAttacks)
                    {
                        if (myAttack && !enemyAttack)
                        {
                            var attackInfo = new ExtraEffectInfo.DamageInfo(extraInf.ctx, 4,
                                $"{gameHandler.players[curPlayer].name}'s Power Glove triggers, dealing 4 damage, ");

                            await gameHandler.players[enemy].TakeDamage(gameHandler, curPlayer, enemy, attackInfo);

                            var info = extraInf as ExtraEffectInfo.AfterAttackInfo;
                            info.output.Add(attackInfo.output);
                        }
                        enemyAttack = true;
                    }
                }
            }
        }

        [Upgrade]
        public class FreezerHeater : Upgrade
        {
            public FreezerHeater() :
                base(UpgradeSet.TinyInventions, "Freezer Heater", 3, 1, 4, Rarity.Epic, "Battlecry: Unfreeze all minions in the shop by 1 turn. Gain +1/+1 for each.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var shop = gameHandler.players[curPlayer].shop.GetAllUpgrades();

                    foreach (var upgrade in shop)
                    {
                        if (upgrade.creatureData.staticKeywords[StaticKeyword.Freeze] > 0)
                        {
                            upgrade.creatureData.staticKeywords[StaticKeyword.Freeze]--;
                            gameHandler.players[curPlayer].creatureData.attack++;
                            gameHandler.players[curPlayer].creatureData.health++;
                        }
                    }

                    return Task.CompletedTask;
                }
            }

        }
    
        [Upgrade]
        public class NanoDuplicatorV10 : Upgrade
        {
            public NanoDuplicatorV10() :
                base(UpgradeSet.TinyInventions, "Nano-Duplicator v10", 1, 1, 1, Rarity.Legendary, "1-Cost Upgrades in your shop have Binary.")
            {
                this.effects.Add(new OnBuyingAMech());
            }
            private class OnBuyingAMech : Effect
            {
                public OnBuyingAMech() : base(EffectType.OnBuyingAMech, "1-Cost Upgrades in your shop have Binary.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (caller is Upgrade u && caller.Cost == 1)
                    {
                        u.creatureData.staticKeywords[StaticKeyword.Binary] = Math.Max(1, u.creatureData.staticKeywords[StaticKeyword.Binary]);
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class CelsiorX : Upgrade
        {
            public CelsiorX() :
                base(UpgradeSet.TinyInventions, "Celsior X", 3, 2, 2, Rarity.Legendary, "Battlecry: Add a 1-cost Absolute Zero to your hand. It Freezes an Upgrade and can be played any number of times.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].hand.AddCard(new AbsoluteZero());
                    return Task.CompletedTask;
                }
            }

            [Token]
            public class AbsoluteZero : Spell
            {
                public AbsoluteZero() :
                    base("Absolute Zero", 1, "Freeze an Upgrade. Return this to your hand.")
                {
                    this.effects.Add(new OnPlay());
                }
                private class OnPlay : Effect
                {
                    public OnPlay() : base(EffectType.OnPlay) { }

                    public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        await PlayerInteraction.FreezeUpgradeInShopAsync(gameHandler, curPlayer, enemy, extraInf.ctx);
                        
                        gameHandler.players[curPlayer].hand.AddCard(new AbsoluteZero());
                    }
                }
            }
        }
    }
}
