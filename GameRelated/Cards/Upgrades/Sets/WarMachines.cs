using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets
{
    public class WarMachines
    {
        [Upgrade]
        public class ArmOfExotron : Upgrade
        {            
            public ArmOfExotron() : 
                base(UpgradeSet.WarMachines, "Arm of Exotron", 2, 2, 1, Rarity.Common, "Battlecry: Gain +2 Spikes.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
                    return Task.CompletedTask;
                }
            }
            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

                string ret = base.GetInfo(gameHandler, player);

                if (list.Count() > 0) ret += " *(played before)*";

                return ret;
            }
        }

        [Upgrade]
        public class LegOfExotron : Upgrade
        {
            public LegOfExotron() :
                base(UpgradeSet.WarMachines, "Leg of Exotron", 2, 1, 2, Rarity.Common, "Battlecry: Gain +2 Shields.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
                    return Task.CompletedTask;
                }
            }
            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

                string ret = base.GetInfo(gameHandler, player);

                if (list.Count() > 0) ret += " *(played before)*";

                return ret;
            }
        }

        [Upgrade]
        public class WheelOfExotron : Upgrade
        {
            public WheelOfExotron() :
                base(UpgradeSet.WarMachines, "Wheel of Exotron", 2, 1, 1, Rarity.Common, "Battlecry: Gain +2 Spikes and +2 Shields.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
                    return Task.CompletedTask;
                }
            }
            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

                string ret = base.GetInfo(gameHandler, player);

                if (list.Count() > 0) ret += " *(played before)*";

                return ret;
            }
        }

        [Upgrade]
        public class MotherboardOfExotron : Upgrade
        {
            public MotherboardOfExotron() :
                base(UpgradeSet.WarMachines, "Motherboard of Exotron", 2, 2, 1, Rarity.Common, "Tiebreaker. Overload: (1)")
            {
                this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
            }
            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

                string ret = base.GetInfo(gameHandler, player);

                if (list.Count() > 0) ret += " *(played before)*";

                return ret;
            }
        }

        [Upgrade]
        public class HeavyDutyPlating : Upgrade
        {
            public HeavyDutyPlating() :
                base(UpgradeSet.WarMachines, "Heavy-Duty Plating", 3, 5, 5, Rarity.Common, "Taunt x2")
            {
                this.creatureData.staticKeywords[StaticKeyword.Taunt] = 2;
            }
        }

        [Upgrade]
        public class SixpistolConstable : Upgrade
        {
            public SixpistolConstable() :
                base(UpgradeSet.WarMachines, "Sixpistol Constable", 15, 6, 6, Rarity.Common, "Rush x6")
            {
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 6;
            }
        }

        [Upgrade]
        public class HelicopterBlades : Upgrade
        {
            public HelicopterBlades() :
                base(UpgradeSet.WarMachines, "Helicopter Blades", 5, 4, 3, Rarity.Common, "Rush. Battlecry: Gain +4 Spikes. Overload: (3)")
            {
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class TankThreads : Upgrade
        {
            public TankThreads() :
                base(UpgradeSet.WarMachines, "Tank Threads", 2, 3, 4, Rarity.Common, "Taunt. Battlecry: Gain +4 Shields. Overload: (3)")
            {
                this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class CopperplatedPrince : Upgrade
        {
            public CopperplatedPrince() :
                base(UpgradeSet.WarMachines, "Copperplated Prince", 3, 4, 1, Rarity.Rare, "Start of Combat: Gain +2 Health for each unspent Mana you have.")
            {
                this.effects.Add(new StartOfCombat());
            }

            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Gain +2 Health for each unspent Mana you have.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.health += 2 * gameHandler.players[curPlayer].curMana;

                    if (extraInf is ExtraEffectInfo.StartOfCombatInfo info)
                    {
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Copperplated Prince gives it +{2 * gameHandler.players[curPlayer].curMana} Health, leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class CopperplatedPrincess : Upgrade
        {
            public CopperplatedPrincess() :
                base(UpgradeSet.WarMachines, "Copperplated Princess", 3, 1, 4, Rarity.Rare, "Start of Combat: Gain +2 Attack for each unspent Mana you have.")
            {
                this.effects.Add(new StartOfCombat());
            }

            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Gain +2 Attack for each unspent Mana you have.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack += 2 * gameHandler.players[curPlayer].curMana;

                    if (extraInf is ExtraEffectInfo.StartOfCombatInfo info)
                    {
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Copperplated Princess gives it +{2 * gameHandler.players[curPlayer].curMana} Attack,  leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class HomingMissile : Upgrade
        {
            public HomingMissile() :
                base(UpgradeSet.WarMachines, "Homing Missile", 4, 3, 3, Rarity.Rare, "Aftermath: Reduce the Health of your opponent's Mech by 5 (but not below 1).")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: Reduce the Health of your opponent's Mech by 5 (but not below 1).", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[enemy].creatureData.health -= 5;
                    if (gameHandler.players[enemy].creatureData.health < 1) gameHandler.players[enemy].creatureData.health = 0;

                    gameHandler.players[enemy].aftermathMessages.Add(
                        $"{gameHandler.players[curPlayer].name}'s Homing Missile reduced your Mech's Health to {gameHandler.players[enemy].creatureData.health}.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class Hypnodrone : Upgrade
        {
            public Hypnodrone() :
                base(UpgradeSet.WarMachines, "Hypnodrone", 7, 6, 6, Rarity.Rare, "Aftermath: Reduce the Attack of your opponent's Mech by 5 (but not below 1).")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: Reduce the Attack of your opponent's Mech by 5 (but not below 1).", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[enemy].creatureData.attack -= 5;
                    if (gameHandler.players[enemy].creatureData.attack < 1) gameHandler.players[enemy].creatureData.attack = 0;

                    gameHandler.players[enemy].aftermathMessages.Add(
                        $"{gameHandler.players[curPlayer].name}'s Hypnodrone reduced your Mech's Attack to {gameHandler.players[enemy].creatureData.attack}.");

                    return Task.CompletedTask;
                }
            }
        }

        //TODO: Add Power Prowler
        
        [Upgrade]
        public class TestMissile : Upgrade
        {
            public TestMissile() : 
                base(UpgradeSet.WarMachines, "Test Missile", 4, 3, 4, Rarity.Rare, "Binary. At the end of your turn in your hand, give this +1/+1.")
            {
                this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
                this.effects.Add(new EndOfTurn());
            }
            private class EndOfTurn : Effect
            {
                public EndOfTurn() : base(EffectType.EndOfTurnInHand) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (caller is Upgrade u)
                    {
                        u.creatureData.attack++;
                        u.creatureData.health++;
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class CopperCommander : Upgrade
        {
            public CopperCommander() :
                base(UpgradeSet.WarMachines, "Copper Commander", 3, 3, 3, Rarity.Epic, "Your Start of Combat effects trigger twice.")
            {
                this.effects.Add(new OnPlay());
            }
            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay, "Your Start of Combat effects trigger twice.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].specificEffects.multiplierStartOfCombat = 2;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class PartAssembler : Upgrade
        {
            public PartAssembler() : 
                base(UpgradeSet.WarMachines, "Part Assembler", 4, 3, 3, Rarity.Epic, "Costs (2) less while you have Spikes. Costs (2) less while you have Shields.")
            {

            }

            public override bool CanBeBought(int shopPos, GameHandler gameHandler, ulong curPlayer, ulong enemy)
            {
                if (shopPos >= gameHandler.players[curPlayer].shop.LastIndex) return false;
                if (this.name == BlankUpgrade.name) return false;
                if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 0) return false;
                //if (this.inLimbo) return false;

                int red = 0;
                if (gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] > 0) red += 2;
                if (gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] > 0) red += 2;

                if (this.Cost - red > gameHandler.players[curPlayer].curMana) return false;

                return true;
            }

            public override Task<bool> BuyCard(int shopPos, GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
            {
                if (gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] > 0) this.Cost -= 2;
                if (gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] > 0) this.Cost -= 2;

                return base.BuyCard(shopPos, gameHandler, curPlayer, enemy, ctx);
            }

            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                int red = 0;

                if (gameHandler.players[player].creatureData.staticKeywords[StaticKeyword.Shields] > 0) red += 2;
                if (gameHandler.players[player].creatureData.staticKeywords[StaticKeyword.Spikes] > 0) red += 2;

                int oldCost = this.Cost;
                this.Cost -= red;

                string ret = base.GetInfo(gameHandler, player);

                this.Cost = oldCost;

                return ret;
            }
        }

        [Upgrade]
        public class OgrimmarJuggernaut : Upgrade
        {
            public OgrimmarJuggernaut() :
                base(UpgradeSet.WarMachines, "Ogrimmar Juggernaut", 7, 6, 5, Rarity.Epic, "Aftermath: Give your opponent a Mine. Unless they play it, it explodes for 10 damage next turn.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: Give your opponent a Mine. Unless they play it, it explodes for 10 damage next turn.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[enemy].hand.AddCard(new OgrimmarJuggernaut.Mine());
                    gameHandler.players[enemy].aftermathMessages.Add(
                         $"{gameHandler.players[curPlayer].name}'s Ogrimmar Juggernaut added a Mine to your hand.");
                    return Task.CompletedTask;
                }
            }

            [Token]
            public class Mine : Spell
            {
                public Mine() : 
                    base("Mine", 6, "If this is unplayed at the end of your turn, it explodes and deals 10 damage to your Mech.")
                {
                    this.effects.Add(new EndOfTurn());
                }
                private class EndOfTurn : Effect
                {
                    public EndOfTurn() : base(EffectType.EndOfTurnInHand) { }

                    public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        var info = extraInf as ExtraEffectInfo.CardInHandInfo;

                        gameHandler.players[curPlayer].creatureData.health = Math.Max(1, gameHandler.players[curPlayer].creatureData.health - 10);
                        gameHandler.players[curPlayer].hand.RemoveCard(info.handPos);

                        //add end of turn public message
                        //$"A Mine explodes in {gameHandler.players[curPlayer].name}'s hand, dealing 10 damage to it."

                        return Task.CompletedTask;
                    }
                }
            }
        }

        [Upgrade]
        public class PanicButton : Upgrade
        {
            public PanicButton() :
                base(UpgradeSet.WarMachines, "Panic Button", 4, 3, 3, Rarity.Epic, "After your Mech is reduced to 5 or less Health, deal 10 damage to the enemy Upgrade.")
            {
                this.effects.Add(new AfterTakingDamage());
            }
            private class AfterTakingDamage : Effect
            {
                public AfterTakingDamage() : base(EffectType.AfterThisTakesDamage, "After your Mech is reduced to 5 or less Health, deal 10 damage to the enemy Upgrade.", EffectDisplayMode.Public) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (gameHandler.players[curPlayer].creatureData.health <= 5)
                    {
                        ExtraEffectInfo.DamageInfo damageInfo = new ExtraEffectInfo.DamageInfo(extraInf.ctx, 10, $"{gameHandler.players[curPlayer].name}'s Panic Button triggers, dealing 10 damage, ");
                        await gameHandler.players[enemy].TakeDamage(gameHandler, curPlayer, enemy, damageInfo);

                        var info = extraInf as ExtraEffectInfo.AfterAttackInfo;
                        info.output.Add(damageInfo.output);

                        this._toBeRemoved = true;
                    }
                }
            }
        }
    
        [Upgrade]
        public class ExotronTheForbidden : Upgrade
        {
            public ExotronTheForbidden() :
                base(UpgradeSet.WarMachines, "Exotron the Forbidden", 15, 15, 15, Rarity.Legendary, "Start of Combat: If you've bought all 5 parts of Exotron this game, destroy the enemy Mech.")
            {
                this.effects.Add(new StartOfCombat());   
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: If you've bought all 5 parts of Exotron this game, destroy the enemy Mech.", EffectDisplayMode.Public) { }

                private bool Criteria(Card m)
                {
                    if (m.name.Equals("Arm of Exotron")) return true;
                    if (m.name.Equals("Leg of Exotron")) return true;
                    if (m.name.Equals("Motherboard of Exotron")) return true;
                    if (m.name.Equals("Wheel of Exotron")) return true;
                    return false;
                }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, this.Criteria);

                    int arm = 0, leg = 0, mb = 0, wheel = 0;
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (list[i].name.Equals("Arm of Exotron")) arm = 1;
                        else if (list[i].name.Equals("Leg of Exotron")) leg = 1;
                        else if (list[i].name.Equals("Motherboard of Exotron")) mb = 1;
                        else if (list[i].name.Equals("Wheel of Exotron")) wheel = 1;
                    }

                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                    if (arm + leg + mb + wheel == 4)
                    {
                        gameHandler.players[enemy].destroyed = true;
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Exotron the Forbidden triggers, destroying {gameHandler.players[enemy].name}.");
                    }
                    else
                    {
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Exotron the Forbidden fails to trigger.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class PulsefireUltracannon : Upgrade
        {
            public PulsefireUltracannon() :
                base(UpgradeSet.WarMachines, "Pulsefire Ultracannon", 10, 10, 7, Rarity.Legendary, "Permanent Start of Combat: Deal 2 damage to the enemy Mech. Improved by 2 after you buy an Upgrade.")
            {
                this.effects.Add(new PulsefireEffect());
            }
            private class PulsefireEffect : Effect
            {
                private int dmg = 2;

                public PulsefireEffect() : base(new EffectType[] { EffectType.StartOfCombat, EffectType.OnBuyingAMech, EffectType.AftermathMe }, "Permanent Start of Combat: Deal 2 damage to the enemy Mech. Improved by 2 after you buy an Upgrade.", EffectDisplayMode.Public) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (extraInf.calledEffect == EffectType.OnBuyingAMech)
                    {
                        this.dmg += 2;
                        this.effectText = $"Permanent Start of Combat: Deal {this.dmg} damage to the enemy Mech. Improved by 2 after you buy an Upgrade.";
                    }
                    else if (extraInf.calledEffect == EffectType.StartOfCombat)
                    {
                        var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                        var dmgInfo = new ExtraEffectInfo.DamageInfo(extraInf.ctx, this.dmg, $"{gameHandler.players[curPlayer].name}'s Pulsefire Ultracannon deals {this.dmg} damage, ");
                        await gameHandler.players[enemy].TakeDamage(gameHandler, curPlayer, enemy, dmgInfo);
                        info.output.Add(dmgInfo.output);
                    }
                    else if (extraInf.calledEffect == EffectType.AftermathMe)
                    {
                        gameHandler.players[curPlayer].nextRoundEffects.Add(new PulsefireEffect { dmg = this.dmg });
                    }
                }

                public override Effect Copy()
                {
                    PulsefireEffect ret = (PulsefireEffect)base.Copy();
                    ret.dmg = this.dmg;
                    return ret;
                }
            }
        }
    }
}
