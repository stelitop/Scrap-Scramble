using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets
{
    public class EdgeOfScience
    {
        [Upgrade]
        public class OrbitalMechanosphere : Upgrade
        {
            public OrbitalMechanosphere() :
                base(UpgradeSet.EdgeOfScience, "Orbital Mechanosphere", 22, 33, 33, Rarity.Common, string.Empty) { }            
        }

        [Upgrade]
        public class GiantPhoton : Upgrade
        {
            public GiantPhoton() : 
                base(UpgradeSet.EdgeOfScience, "Giant Photon", 4, 5, 4, Rarity.Common, "Rush. Overload: (3)")
            {                
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
            }
        }

        [Upgrade]
        public class PoolOfBronze : Upgrade
        {
            public PoolOfBronze() :
                base(UpgradeSet.EdgeOfScience, "Pool of Bronze", 2, 6, 2, Rarity.Common, "Aftermath: Replace your shop with 6 Common Upgrades.")
            {
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Replace your shop with 6 Common Upgrades.", EffectDisplayMode.Private) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].shop.Clear();

                    List<Upgrade> subList = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Common && x.Cost <= gameHandler.players[curPlayer].maxMana - 5);
                    for (int i = 0; i < 6; i++)
                    {
                        Upgrade m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                        gameHandler.players[curPlayer].shop.AddUpgrade(m);
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class StasisCrystal : Upgrade
        {
            public StasisCrystal() :
                base(UpgradeSet.EdgeOfScience, "Stasis Crystal", 2, 0, 2, Rarity.Common, "Battlecry: Increase your Maximum Mana by 1.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].maxMana++;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class VoltageTracker : Upgrade
        {
            public VoltageTracker() :
                base(UpgradeSet.EdgeOfScience, "Voltage Tracker", 4, 2, 3, Rarity.Common, "Battlecry: Gain +1/+1 for each Overloaded Mana Crystal you have.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack +=
                        gameHandler.players[curPlayer].overloaded + gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload];
                    gameHandler.players[curPlayer].creatureData.health +=
                        gameHandler.players[curPlayer].overloaded + gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload];

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class TrafficCone : Upgrade
        {
            public TrafficCone() :
                base(UpgradeSet.EdgeOfScience, "Traffic Cone", 2, 2, 1, Rarity.Common, "Binary. Battlecry: Gain +2 Spikes. Overload: (1)")
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
        }

        [Upgrade]
        public class ShieldbotClanker : Upgrade
        {
            public ShieldbotClanker() :
                base(UpgradeSet.EdgeOfScience, "Shieldbot Clanker", 5, 2, 4, Rarity.Common, "Battlecry and Aftermath : Gain +8 Shields.")
            {
                this.effects.Add(new Battlecry());
                this.effects.Add(new Aftermath());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
                    return Task.CompletedTask;
                }
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Gain +8 Shields.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Shieldbot Clanker gives you +8 Shields.");
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class SpikebotShanker : Upgrade
        {
            public SpikebotShanker() :
                base(UpgradeSet.EdgeOfScience, "Spikebot Shanker", 5, 4, 2, Rarity.Common, "Battlecry and Aftermath : Gain +8 Spikes.")
            {
                this.effects.Add(new Battlecry());
                this.effects.Add(new Aftermath());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 8;
                    return Task.CompletedTask;
                }
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Gain +8 Spikes.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 8;
                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Spikebot Shanker gives you +8 Spikes.");
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class IndecisiveAutoshopper : Upgrade
        {
            public IndecisiveAutoshopper() :
                base(UpgradeSet.EdgeOfScience, "Indecisive Autoshopper", 4, 2, 4, Rarity.Rare, "Binary. Battlecry: Refresh your shop.")
            {
                this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].shop.Refresh(gameHandler, gameHandler.players[curPlayer].pool, gameHandler.players[curPlayer].maxMana, false);
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class LightChaser : Upgrade
        {
            public LightChaser() :
                base(UpgradeSet.EdgeOfScience, "Light Chaser", 12, 9, 7, Rarity.Rare, "Battlecry: Gain Rush x1 for each Overloaded Mana Crystal you have.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush] +=
                        gameHandler.players[curPlayer].overloaded + gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload];

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class SocietyProgressor : Upgrade
        {
            public SocietyProgressor() :
                base(UpgradeSet.EdgeOfScience, "Society Progressor", 4, 1, 7, Rarity.Rare, "Aftermath: Remove Binary from all Upgrades in your opponent's shop.")
            {
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: Remove Binary from all Upgrades in your opponent's shop.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (curPlayer == enemy) return Task.CompletedTask;
                    var upgrades = gameHandler.players[enemy].shop.GetAllUpgrades();

                    foreach (var upgrade in upgrades)
                    {
                        if (upgrade.creatureData.staticKeywords[StaticKeyword.Binary] > 0)
                        {
                            upgrade.creatureData.staticKeywords[StaticKeyword.Binary] = 0;
                            upgrade.cardText += " (No Binary)";
                        }
                    }

                    gameHandler.players[enemy].aftermathMessages.Add(
                        $"{gameHandler.players[curPlayer].name}'s Society Progressor removed Binary from all Upgrades in your shop.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class EnergeticField : Upgrade
        {
            public EnergeticField() :
                base(UpgradeSet.EdgeOfScience, "Energetic Field", 3, 3, 3, Rarity.Rare, "Battlecry: If you're Overloaded, add 3 random Upgrades that Overload to your hand.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (gameHandler.players[curPlayer].overloaded > 0 || gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload] > 0)
                    {
                        List<Upgrade> pool = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.creatureData.staticKeywords[StaticKeyword.Overload] > 0);
                        for (int i = 0; i < 3; i++)
                        {
                            int x = GameHandler.randomGenerator.Next(0, pool.Count);
                            gameHandler.players[curPlayer].hand.AddCard(pool[x]);
                        }
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class EvolveOMatic : Upgrade
        {
            public EvolveOMatic() :
                base(UpgradeSet.EdgeOfScience, "Evolve-o-Matic", 5, 4, 5, Rarity.Rare, "Battlecry: Choose an Upgrade in your shop. Discover an Upgrade that costs (1) more to replace it with.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    int chosenIndex = await PlayerInteraction.ChooseUpgradeInShopAsync(gameHandler, curPlayer, enemy, extraInf.ctx);
                    if (chosenIndex == -1) return;

                    int newCost = gameHandler.players[curPlayer].shop.At(chosenIndex).Cost + 1;

                    List<Upgrade> pool = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.Cost == newCost);

                    if (pool.Count > 0)
                    {
                        Upgrade x = (Upgrade)(await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, extraInf.ctx, "Discover an Upgrade to replace with", pool, false));
                        gameHandler.players[curPlayer].shop.TransformUpgrade(chosenIndex, x);
                    }
                }
            }
        }

        [Upgrade]
        public class DevolveOMatic : Upgrade
        {
            public DevolveOMatic() :
                base(UpgradeSet.EdgeOfScience, "Devolve-o-Matic", 5, 5, 4, Rarity.Rare, "Battlecry: Choose an Upgrade in your shop. Discover an Upgrade that costs (1) less to replace it with.")
            {
                this.effects.Add(new Battlecry());                
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    int chosenIndex = await PlayerInteraction.ChooseUpgradeInShopAsync(gameHandler, curPlayer, enemy, extraInf.ctx);
                    if (chosenIndex == -1) return;

                    int newCost = gameHandler.players[curPlayer].shop.At(chosenIndex).Cost - 1;

                    List<Upgrade> pool = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.Cost == newCost);

                    if (pool.Count > 0)
                    {
                        Upgrade x = (Upgrade)(await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, extraInf.ctx, "Discover an Upgrade to replace with", pool, false));
                        gameHandler.players[curPlayer].shop.TransformUpgrade(chosenIndex, x);
                    }
                }
            }
        }

        [Upgrade]
        public class HyperMagneticCloud : Upgrade
        {
            public HyperMagneticCloud() :
                base(UpgradeSet.EdgeOfScience, "Hyper-Magnetic Cloud", 7, 7, 6, Rarity.Epic, "Aftermath: If you're Overloaded for at least 7 Mana, add all 7 Spare Parts to your hand.")
            {
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: If you're Overloaded for at least 7 Mana, add all 7 Spare Parts to your hand.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (gameHandler.players[curPlayer].overloaded >= 7)
                    {
                        foreach (var sp in gameHandler.players[curPlayer].pool.spareparts)
                        {
                            gameHandler.players[curPlayer].hand.AddCard(sp);
                        }
                    }
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class DatabaseCore : Upgrade
        {
            public DatabaseCore() :
                base(UpgradeSet.EdgeOfScience, "Database Core", 6, 4, 5, Rarity.Epic, "Battlecry: Remember your Mech's keywords. Aftermath: Add them to your Mech.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    CreatureData kws = gameHandler.players[curPlayer].creatureData.DeepCopy();

                    Aftermath aftermath = new Aftermath(kws);

                    aftermath.effectText = "Aftermath: Gain ";
                    aftermath.displayMode = EffectDisplayMode.Private;

                    bool checkedFirst = false;

                    foreach (var keyword in kws.staticKeywords)
                    {
                        if (keyword.Value != 0)
                        {
                            if (!checkedFirst)
                            {
                                aftermath.effectText += $"{keyword.Key} x{keyword.Value}";
                                checkedFirst = true;
                            }
                            else
                            {

                                aftermath.effectText += $", {keyword.Key} x{keyword.Value}";
                            }
                        }
                    }

                    aftermath.effectText += ".";

                    gameHandler.players[curPlayer].effects.Add(aftermath);

                    return Task.CompletedTask;
                }

                private class Aftermath : Effect
                {
                    private CreatureData _kw;
                    public Aftermath() : base(EffectType.AftermathMe) { }
                    public Aftermath(CreatureData kw) : base(EffectType.AftermathMe) { this._kw = kw; }

                    public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        string aftermathMsg = "Your Database Core gives you ";

                        bool checkedFirst = false;
                        foreach (var keyword in _kw.staticKeywords)
                        {
                            if (keyword.Value > 0)
                            {
                                gameHandler.players[curPlayer].creatureData.staticKeywords[keyword.Key] += keyword.Value;
                                if (!checkedFirst)
                                {
                                    checkedFirst = true;
                                    aftermathMsg += $"{keyword.Key} x{keyword.Value}";
                                }
                                else
                                {
                                    aftermathMsg += $", {keyword.Key} x{keyword.Value}";
                                }
                            }
                        }

                        gameHandler.players[curPlayer].aftermathMessages.Add(aftermathMsg);

                        return Task.CompletedTask;
                    }
                }
            }
        }
    
        [Upgrade]
        public class MassAccelerator : Upgrade
        {
            public MassAccelerator() :
                base(UpgradeSet.EdgeOfScience, "Mass Accelerator", 6, 5, 5, Rarity.Epic, "Start of Combat: If you're Overloaded, deal 10 damage to the enemy Mech.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: If you're Overloaded, deal 10 damage to the enemy Mech.", EffectDisplayMode.Public) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                    if (gameHandler.players[curPlayer].overloaded > 0 || gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload] > 1)
                    {
                        var dmgInfo = new ExtraEffectInfo.DamageInfo(extraInf.ctx, 10, $"{gameHandler.players[curPlayer].name}'s Mass Accelerator triggers, dealing 10 damage, ");
                        await gameHandler.players[enemy].TakeDamage(gameHandler, curPlayer, enemy, dmgInfo);
                        info.output.Add(dmgInfo.output);
                    }
                    else
                    {
                        info.output.Add($"{gameHandler.players[curPlayer].name}'s Mass Accelerator failed to trigger.");
                    }                    
                }
            }
        }

        [Upgrade]
        public class PhilosophersStone : Upgrade
        {
            public PhilosophersStone() :
                base(UpgradeSet.EdgeOfScience, "Philosopher's Stone", 3, 1, 1, Rarity.Epic, "Battlecry: Transform your Common Upgrades into random Legendary ones.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<int> shopIndexes = gameHandler.players[curPlayer].shop.GetAllUpgradeIndexes();

                    List<Upgrade> legendaries = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Legendary);

                    foreach (var index in shopIndexes)
                    {
                        if (gameHandler.players[curPlayer].shop.At(index).rarity == Rarity.Common)
                        {
                            gameHandler.players[curPlayer].shop.TransformUpgrade(index, legendaries[GameHandler.randomGenerator.Next(legendaries.Count)]);
                        }
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class ParadoxEngine : Upgrade
        {
            public ParadoxEngine() :
                base(UpgradeSet.EdgeOfScience, "Paradox Engine", 12, 10, 10, Rarity.Legendary, "After you buy an Upgrade, refresh your shop.")
            {
                this.effects.Add(new OnBuyingAMinion());
            }

            private class OnBuyingAMinion : Effect
            {
                public OnBuyingAMinion() : base(EffectType.OnBuyingAMech, "After you buy an Upgrade, refresh your shop.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].shop.Refresh(gameHandler, gameHandler.players[curPlayer].pool, gameHandler.players[curPlayer].maxMana, false);
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class EarthsPrototypeCore : Upgrade
        {
            public EarthsPrototypeCore() :
                base(UpgradeSet.EdgeOfScience, "Earth's Prototype Core", 7, 0, 12, Rarity.Legendary, "Battlecry: For each Overload Upgrade applied to your Mech this game, increase your Maximum Mana by 1.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                private bool criteria(Card c)
                {
                    if (c is Upgrade u)
                    {
                        return (u.creatureData.staticKeywords[StaticKeyword.Overload] > 0);
                    }

                    return false;
                }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    int bonus = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, this.criteria).Count;

                    gameHandler.players[curPlayer].maxMana += bonus;

                    return Task.CompletedTask;
                }
            }
        }
    }
}
