using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets
{
    public class VentureCo
    {
        [Upgrade]
        public class Shieldmobile : Upgrade
        {
            public Shieldmobile() :
                base(UpgradeSet.VentureCo, "Shieldmobile", 2, 2, 6, Rarity.Common, "Magnetic. Battlecry: Gain +6 Shields. Overload: (4).")
            {
                this.effects.Add(new Battlecry());
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
                this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 6;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class Spikecycle : Upgrade
        {
            public Spikecycle() :
                base(UpgradeSet.VentureCo, "Spikecycle", 2, 6, 2, Rarity.Common, "Magnetic. Battlecry: Gain +6 Spikes. Overload: (4).")
            {
                this.effects.Add(new Battlecry());
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
                this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 6;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class SiliconGrenadeBelt : Upgrade
        {
            public SiliconGrenadeBelt() :
                base(UpgradeSet.VentureCo, "Silicon Grenade Belt", 3, 4, 2, Rarity.Common, "Start of Combat: Deal 1 damage to the enemy Mech, twice.")
            {
                this.effects.Add(new StartOfCombat());
            }

            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Deal 1 damage to the enemy Mech, twice.", EffectDisplayMode.Public) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                    var dmgInfo = new ExtraEffectInfo.DamageInfo(extraInf.ctx, 1, $"{gameHandler.players[curPlayer].name}'s Silicon Grenade Belt deals 1 damage, ");
                    await gameHandler.players[enemy].TakeDamage(gameHandler, curPlayer, enemy, dmgInfo);
                    info.output.Add(dmgInfo.output);

                    dmgInfo = new ExtraEffectInfo.DamageInfo(extraInf.ctx, 1, $"{gameHandler.players[curPlayer].name}'s Silicon Grenade Belt deals 1 damage, ");
                    await gameHandler.players[enemy].TakeDamage(gameHandler, curPlayer, enemy, dmgInfo);
                    info.output.Add(dmgInfo.output);
                }
            }
        }

        [Upgrade]
        public class VentureCoPauldrons : Upgrade
        {
            public VentureCoPauldrons() :
                base(UpgradeSet.VentureCo, "Venture Co. Pauldrons", 4, 2, 2, Rarity.Common, "Taunt. Battlecry: Gain +1/+1 for each Venture Co. Upgrade you've bought this game.")
            {
                this.effects.Add(new Battlecry());
                this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, x => x.name.Contains("Venture Co."));
                    gameHandler.players[curPlayer].creatureData.attack += list.Count();
                    gameHandler.players[curPlayer].creatureData.health += list.Count();

                    return Task.CompletedTask;
                }
            }

            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                return base.GetInfo(gameHandler, player) +
                    $" *({CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, x => x.name.Contains("Venture Co.")).Count})*";
            }
        }

        [Upgrade]
        public class VentureCoSawblade : Upgrade
        {
            public VentureCoSawblade() :
                base(UpgradeSet.VentureCo, "Venture Co. Sawblade", 3, 1, 2, Rarity.Common, "Battlecry: Gain +1 Attack for each Venture Co. Upgrade you've bought this game.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, x => x.name.Contains("Venture Co."));
                    gameHandler.players[curPlayer].creatureData.attack += list.Count();

                    return Task.CompletedTask;
                }
            }

            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                return base.GetInfo(gameHandler, player) +
                    $" *({CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, x => x.name.Contains("Venture Co.")).Count})*";
            }
        }

        [Upgrade]
        public class VentureCoThruster : Upgrade
        {
            public VentureCoThruster() :
                base(UpgradeSet.VentureCo, "Venture Co. Thruster", 6, 1, 1, Rarity.Common, "Rush. Battlecry: Gain +1/+1 for each Venture Co. Upgrade you've bought this game.")
            {
                this.effects.Add(new Battlecry());
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, x => x.name.Contains("Venture Co."));
                    gameHandler.players[curPlayer].creatureData.attack += list.Count();
                    gameHandler.players[curPlayer].creatureData.health += list.Count();

                    return Task.CompletedTask;
                }
            }

            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                return base.GetInfo(gameHandler, player) +
                    $" *({CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, x => x.name.Contains("Venture Co.")).Count})*";
            }
        }

        [Upgrade]
        public class VentureCoCoolant : Upgrade
        {
            public VentureCoCoolant() :
                base(UpgradeSet.VentureCo, "Venture Co. Coolant", 2, 2, 3, Rarity.Common, "Battlecry: Freeze an Upgrade. Give it -2 Attack. Overload: (1)")
            {
                this.effects.Add(new Battlecry());
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

                    Upgrade chosen = await PlayerInteraction.FreezeUpgradeInShopAsync(gameHandler, curPlayer, enemy, extraInf.ctx);

                    chosen.creatureData.attack -= 2;
                    if (chosen.creatureData.attack < 0) chosen.creatureData.attack = 0;
                }
            }
        }

        [Upgrade]
        public class VentureCoSticker : Upgrade
        {
            public VentureCoSticker() :
                base(UpgradeSet.VentureCo, "Venture Co. Sticker", 1, 0, 2, Rarity.Common, string.Empty)
            {

            }
        }

        [Upgrade]
        public class AnonymousSupplier : Upgrade
        {
            public AnonymousSupplier() :
                base(UpgradeSet.VentureCo, "Anonymous Supplier", 3, 3, 3, Rarity.Rare, "Your Upgrades are not shared in the combat log.")
            {
                this.effects.Add(new OnPlay());
            }
            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].specificEffects.hideUpgradesInLog = true;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class CopyShredder : Upgrade
        {
            public CopyShredder() :
                base(UpgradeSet.VentureCo, "Copy Shredder", 3, 2, 4, Rarity.Rare, "Start of Combat: Gain +1/+1 for each duplicate Upgrade your opponent bought this round.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Gain +1/+1 for each duplicate Upgrade your opponent bought this round.", EffectDisplayMode.Public) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<string> found = new List<string>();
                    int buff = 0;

                    for (int i = 0; i < gameHandler.players[enemy].buyHistory.Last().Count(); i++)
                    {
                        if (found.Contains(gameHandler.players[enemy].buyHistory.Last()[i].name))
                        {
                            buff++;
                        }
                        else found.Add(gameHandler.players[enemy].buyHistory.Last()[i].name);
                    }

                    gameHandler.players[curPlayer].creatureData.attack += buff;
                    gameHandler.players[curPlayer].creatureData.health += buff;

                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                    info.output.Add(
                        $"{gameHandler.players[curPlayer].name}'s Copy Shredder gives it +{buff}/+{buff}, leaving it as s {gameHandler.players[curPlayer].creatureData.Stats()}.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class IllegalThermodynamo : Upgrade
        {
            public IllegalThermodynamo() :
                base(UpgradeSet.VentureCo, "Illegal Thermodynamo", 3, 2, 2, Rarity.Rare, "After this is Frozen, gain +3/+3.")
            {
                this.effects.Add(new OnBeingFrozen());
            }
            private class OnBeingFrozen : Effect
            {
                public OnBeingFrozen() : base(EffectType.OnBeingFrozen) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (caller is Upgrade u)
                    {
                        u.creatureData.attack += 3;
                        u.creatureData.health += 3;
                    }

                    return Task.CompletedTask;
                }
            }
            
        }

        [Upgrade]
        public class TreasureMiner : Upgrade
        {
            public TreasureMiner() :
                base(UpgradeSet.VentureCo, "Treasure Miner", 5, 4, 3, Rarity.Rare, "Battlecry: Discover a Legendary Upgrade.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Upgrade> pool = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Legendary);
                    await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, extraInf.ctx, "Discover a Legendary Upgrade", pool);
                }
            }
        }

        [Upgrade]
        public class VentureCoVault : Upgrade
        {
            public VentureCoVault() :
                base(UpgradeSet.VentureCo, "Venture Co. Vault", 3, 0, 6, Rarity.Rare, "Taunt. Aftermath: Add 3 other random Venture Co. Upgrades to your shop.")
            {
                this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Add 3 other random Venture Co. Upgrades to your shop.", EffectDisplayMode.Private) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Upgrade> list = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.name.Contains("Venture Co."));

                    for (int i = list.Count() - 1; i >= 0; i--)
                    {
                        if (list[i].name.Equals("Venture Co. Vault"))
                        {
                            list.RemoveAt(i);
                            break;
                        }
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        int card = GameHandler.randomGenerator.Next(0, list.Count());

                        gameHandler.players[curPlayer].shop.AddUpgrade(list[card]);
                    }


                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Venture Co. Vault adds 3 random Venture Co. Upgrades to your shop.");
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class VentureCoExcavator : Upgrade
        {
            public VentureCoExcavator() :
                base(UpgradeSet.VentureCo, "Venture Co. Excavator", 7, 5, 6, Rarity.Rare, "Magnetic. If your hand is empty, Magnetic x3 instead.")
            {
                this.effects.Add(new OnPlay());
            }
            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (gameHandler.players[curPlayer].hand.OptionsCount() == 0)
                    {
                        if (caller is Upgrade u)
                        {
                            u.creatureData.staticKeywords[StaticKeyword.Magnetic] = 3;
                        }
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class EncodedSwitchboard : Upgrade
        {
            public EncodedSwitchboard() :
                base(UpgradeSet.VentureCo, "Encoded Switchboard", 3, 0, 4, Rarity.Epic, "Aftermath: Give a random Legendary Upgrade in your shop \"Combo: Gain \'Spellburst: Gain Rush x4.\'\"")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Give a random Legendary Upgrade in your shop \"Combo: Gain \'Spellburst: Gain Rush x4.\'\"", EffectDisplayMode.Private) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Upgrade> legendaries = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].shop.GetAllUpgrades(), x => x.rarity == Rarity.Legendary);
                    int index = GameHandler.randomGenerator.Next(0, legendaries.Count());

                    legendaries[index].effects.Add(new SwitchboardEffect());
                    legendaries[index].cardText += " Combo: Gain \'Spellburst: Gain Rush x4.\'";

                    return Task.CompletedTask;
                }

                private class SwitchboardEffect : Effect
                {
                    public SwitchboardEffect() : base(EffectType.Combo) { }

                    public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        gameHandler.players[curPlayer].effects.Add(new Spellburst());
                        return Task.CompletedTask;
                    }
                    private class Spellburst : Effect
                    {
                        public Spellburst() : base(EffectType.Spellburst, "Spellburst: Gain Rush x4.", EffectDisplayMode.Private) { }
                        public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                        {
                            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush] += 4;
                            return Task.CompletedTask;
                        }
                    }
                }                
            }
        }
    
        [Upgrade]
        public class Investotron : Upgrade
        {
            public Investotron() :
                base(UpgradeSet.VentureCo, "Investotron", 5, 4, 4, Rarity.Epic, "Aftermath: Transform a random Upgrade in your shop into an Investotron. Give your Upgrade +4/+4.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Transform a random Upgrade in your shop into an Investotron. Give your Upgrade +4/+4.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return Task.CompletedTask;

                    int index = gameHandler.players[curPlayer].shop.GetRandomUpgradeIndex();
                    gameHandler.players[curPlayer].shop.TransformUpgrade(index, new VentureCo.Investotron());
                    gameHandler.players[curPlayer].creatureData.attack += 4;
                    gameHandler.players[curPlayer].creatureData.health += 4;

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Investotron transforms a random Upgrade in your shop into an Investotron and gives you +4/+4, leaving you as a {gameHandler.players[curPlayer].creatureData.Stats()}.");

                    return Task.CompletedTask;
                }
            }
        }
    
        [Upgrade]
        public class SponsorshipScrubber : Upgrade
        {
            public SponsorshipScrubber() :
                base(UpgradeSet.VentureCo, "Sponsorship Scrubber", 3, 1, 2, Rarity.Epic, "Start of Combat: If your opponent has purchased a Venture Co. Upgrade this game, steal 6 Attack from their Upgrade.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: If your opponent has purchased a Venture Co. Upgrade this game, steal 6 Attack from their Upgrade.", EffectDisplayMode.Public) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[enemy].playHistory, x => x.name.Contains("Venture Co."));

                    ExtraEffectInfo.StartOfCombatInfo info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                    if (list.Count() > 0)
                    {
                        int stolen = Math.Min(6, gameHandler.players[enemy].creatureData.attack - 1);
                        gameHandler.players[enemy].creatureData.attack -= stolen;
                        gameHandler.players[curPlayer].creatureData.attack += stolen;
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Sponsorship Scrubbler steals {stolen} Attack from {gameHandler.players[enemy].name}, leaving it with {gameHandler.players[enemy].creatureData.attack} Attack and leaving {gameHandler.players[curPlayer].name} with {gameHandler.players[curPlayer].creatureData.attack} Attack.");
                    }
                    else
                    {
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Sponsorship Scrubbler fails to trigger.");
                    }

                    return Task.CompletedTask;
                }
            }
        }       
    
        [Upgrade]
        public class VentureCoFlamethrower : Upgrade
        {
            public VentureCoFlamethrower() :
                base(UpgradeSet.VentureCo, "Venture Co. Flamethrower", 6, 3, 3, Rarity.Epic, "Start of Combat: Deal 3 damage to the enemy Mech for each Venture Co. Upgrade you've played this game.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Deal 3 damage to the enemy Mech for each Venture Co. Upgrade you've played this game.", EffectDisplayMode.Public) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, x => x.name.Contains("Venture Co."));
                    
                    var info = new ExtraEffectInfo.DamageInfo(extraInf.ctx, 3 * list.Count(), $"{gameHandler.players[curPlayer].name}'s Venture Co. Flamethrower deals {2 * list.Count()} damage, ");

                    await gameHandler.players[enemy].TakeDamage(gameHandler, curPlayer, enemy, info);

                    var SOCinfo = extraInf as ExtraEffectInfo.StartOfCombatInfo;
                    SOCinfo.output.Add(info.output);
                }
            }

            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                return base.GetInfo(gameHandler, player) +
                    $" *({CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, x => x.name.Contains("Venture Co.")).Count})*";
            }
        }
    
        [Upgrade]
        public class GoldPrinceGallyWX : Upgrade
        {
            public GoldPrinceGallyWX() :
                base(UpgradeSet.VentureCo, "Gold Prince Gally-WX", 7, 5, 8, Rarity.Legendary, "After you play a Spare Part, Discover an Upgrade. It costs (2) less.")
            {
                this.effects.Add(new CastingASpell());
            }
            private class CastingASpell : Effect
            {
                public CastingASpell() : base(EffectType.OnSpellCast, "After you play a Spare Part, Discover an Upgrade. It costs (2) less.", EffectDisplayMode.Private) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (caller is Spell s)
                    {
                        if (s.rarity == SpellRarity.Spare_Part)
                        {
                            var choice = await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, extraInf.ctx, "Discover an Upgrade", gameHandler.players[curPlayer].pool.upgrades).ConfigureAwait(false);
                            choice.Cost -= 2;
                        }
                    }
                }
            }
        }
    
        [Upgrade]
        public class NeatoMagnetMagneto : Upgrade
        {
            public NeatoMagnetMagneto() :
                base(UpgradeSet.VentureCo, "Neato Magnet Magneto", 9, 8, 6, Rarity.Legendary, "Spellburst: If the spell is a Spare Part other than Mana Capsule, apply its effect to all Upgrades in your shop.")
            {
                this.effects.Add(new Spellburst());
            }
            private class Spellburst : Effect
            {
                public Spellburst() : base() { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (caller is Spell s)
                    {
                        if (s.rarity == SpellRarity.Spare_Part && s.name != "Mana Capsule")
                        {
                            List<int> upgrades = gameHandler.players[curPlayer].shop.GetAllUpgradeIndexes();

                            for (int i = 0; i < upgrades.Count(); i++)
                            {
                                s.CastOnUpgradeInShop(upgrades[i], gameHandler, curPlayer, enemy, extraInf.ctx);
                            }
                        }
                    }                    

                    return Task.CompletedTask;
                }
            }
        }
    }
}
