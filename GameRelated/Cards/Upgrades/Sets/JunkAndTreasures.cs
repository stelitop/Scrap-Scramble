using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets
{
    public class JunkAndTreasures
    {
        [Upgrade]
        public class BronzeBruiser : Upgrade
        {
            public BronzeBruiser() :
                base(UpgradeSet.JunkAndTreasures, "Bronze Bruiser", 2, 1, 2, Rarity.Common, "Aftermath: Add 4 random Common Upgrades to your shop.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Add 4 random Common Upgrades to your shop.", EffectDisplayMode.Private) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Upgrade> list = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Common && x.Cost <= gameHandler.players[curPlayer].maxMana-5);

                    for (int i = 0; i < 4; i++)
                    {
                        int pos = GameHandler.randomGenerator.Next(0, list.Count());
                        gameHandler.players[curPlayer].shop.AddUpgrade(list[pos]);
                    }

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Bronze Bruiser adds 4 random Common Upgrades to your shop.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class SilverShogun : Upgrade
        {
            public SilverShogun() :
                base(UpgradeSet.JunkAndTreasures, "Silver Shogun", 3, 2, 3, Rarity.Common, "Aftermath: Add 3 random Rare Upgrades to your shop.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Add 3 random Rare Upgrades to your shop.", EffectDisplayMode.Private) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Upgrade> list = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Rare && x.Cost <= gameHandler.players[curPlayer].maxMana - 5);

                    for (int i = 0; i < 3; i++)
                    {
                        int pos = GameHandler.randomGenerator.Next(0, list.Count());
                        gameHandler.players[curPlayer].shop.AddUpgrade(list[pos]);
                    }

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Silver Shogun adds 3 random Rare Upgrades to your shop.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class GoldenGunner : Upgrade
        {
            public GoldenGunner() :
                base(UpgradeSet.JunkAndTreasures, "Golden Gunner", 4, 3, 4, Rarity.Common, "Aftermath: Add 2 random Epic Upgrades to your shop.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Add 2 random Epic Upgrades to your shop.", EffectDisplayMode.Private) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Upgrade> list = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Epic && x.Cost <= gameHandler.players[curPlayer].maxMana - 5);

                    for (int i = 0; i < 2; i++)
                    {
                        int pos = GameHandler.randomGenerator.Next(0, list.Count());
                        gameHandler.players[curPlayer].shop.AddUpgrade(list[pos]);
                    }

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Golden Gunner adds 2 random Epic Upgrades to your shop.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class PlatinumParagon : Upgrade
        {
            public PlatinumParagon() :
                base(UpgradeSet.JunkAndTreasures, "Platinum Paragon", 5, 4, 5, Rarity.Common, "Aftermath: Add 1 random Legendary Upgrade to your shop.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Add 1 random Legendary Upgrade to your shop.", EffectDisplayMode.Private) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Upgrade> list = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Legendary && x.Cost <= gameHandler.players[curPlayer].maxMana - 5);

                    for (int i = 0; i < 1; i++)
                    {
                        int pos = GameHandler.randomGenerator.Next(0, list.Count());
                        gameHandler.players[curPlayer].shop.AddUpgrade(list[pos]);
                    }

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Platinum Paragon adds 1 random Legendary Upgrade to your shop.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class BoomerangMagnet : Upgrade
        {
            public BoomerangMagnet() :
                base(UpgradeSet.JunkAndTreasures, "Boomerang Magnet", 5, 5, 4, Rarity.Common, "Magnetic, Rush. Overload: (4)")
            {
                this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
            }
        }

        [Upgrade]
        public class MagnetBall : Upgrade
        {
            public MagnetBall() :
                base(UpgradeSet.JunkAndTreasures, "Magnet Ball", 2, 4, 5, Rarity.Common, "Magnetic, Taunt. Overload: (4)")
            {
                this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
            }
        }

        [Upgrade]
        public class CircusCircuit : Upgrade
        {
            public CircusCircuit() :
                base(UpgradeSet.JunkAndTreasures, "Circus Circuit", 3, 2, 3, Rarity.Common, "Aftermath: Add a random Spare Part to your hand.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Add a random Spare Part to your hand.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    int pos = GameHandler.randomGenerator.Next(gameHandler.players[curPlayer].pool.spareparts.Count());

                    gameHandler.players[curPlayer].hand.AddCard(gameHandler.players[curPlayer].pool.spareparts[pos]);

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Circus Circuit added a {gameHandler.players[curPlayer].pool.spareparts[pos].name} to your hand.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class TrashCube : Upgrade
        {
            public TrashCube() :
                base(UpgradeSet.JunkAndTreasures, "Trash Cube", 5, 4, 4, Rarity.Common, "Echo, Magnetic")
            {
                this.creatureData.staticKeywords[StaticKeyword.Echo] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            }
        }

        [Upgrade]
        public class EngineersToolbox : Upgrade
        {
            public EngineersToolbox() :
                base(UpgradeSet.JunkAndTreasures, "Engineer's Toolbox", 4, 2, 2, Rarity.Rare, "After you buy an Upgrade, gain +2 Spikes and +2 Shields.")
            {
                this.effects.Add(new OnBuyingAMech());
            }
            private class OnBuyingAMech : Effect
            {
                public OnBuyingAMech() : base(EffectType.OnBuyingAMech, "After you buy an Upgrade, gain +2 Spikes and +2 Shields.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class ScrapStacker : Upgrade
        {
            public ScrapStacker() :
                base(UpgradeSet.JunkAndTreasures, "Scrap Stacker", 8, 4, 4, Rarity.Rare, "After you buy an Upgrade, gain +2/+2.")
            {
                this.effects.Add(new OnBuyingAMech());
            }
            private class OnBuyingAMech : Effect
            {
                public OnBuyingAMech() : base(EffectType.OnBuyingAMech, "After you buy an Upgrade, gain +2/+2.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack += 2;
                    gameHandler.players[curPlayer].creatureData.health += 2;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class LivingIncinerator : Upgrade
        {
            public LivingIncinerator() :
                base(UpgradeSet.JunkAndTreasures, "Living Incinerator", 12, 8, 8, Rarity.Rare, "Battlecry: Destroy all remaining Upgrades in your shop. Gain +1/+1 for each.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].shop.OptionsCount();
                    gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].shop.OptionsCount();

                    gameHandler.players[curPlayer].shop.Clear();

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class PileOfJunk : Upgrade
        {
            public PileOfJunk() :
                base(UpgradeSet.JunkAndTreasures, "Pile of Junk", 6, 3, 3, Rarity.Rare, "Choose One: Gain +1 Attack for each other Upgrade in your shop; or +1 Health.")
            {
                this.effects.Add(new OnPlay());
            }
            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    PlayerInteraction chooseOne = new PlayerInteraction("Choose One", "1) Gain +1 Attack for each other Upgrade in your shop\n2) Gain +1 Health for each other Upgrade in your shop", "Write the corresponding number");
                    string defaultAns = GameHandler.randomGenerator.Next(1, 3).ToString();

                    string ret = await chooseOne.SendInteractionAsync(gameHandler, curPlayer, (x, y, z) => GeneralFunctions.Within(x, 1, 2), defaultAns, extraInf.ctx);

                    if (int.Parse(ret) == 1)
                    {
                        gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].shop.GetAllUpgradeIndexes().Count();
                    }
                    else if (int.Parse(ret) == 2)
                    {
                        gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].shop.GetAllUpgradeIndexes().Count();
                    }
                }
            }
        }

        [Upgrade]
        public class Scrapbarber : Upgrade
        {
            public Scrapbarber() :
                base(UpgradeSet.JunkAndTreasures, "Scrapbarber", 4, 3, 3, Rarity.Rare, "After this attacks the enemy Mech, steal 2 Attack and Health from it.")
            {
                this.effects.Add(new AfterThisAttacks());
            }
            private class AfterThisAttacks : Effect
            {
                public AfterThisAttacks() : base(EffectType.AfterThisAttacks, "After this attacks the enemy Mech, steal 2 Attack and Health from it.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    int stolenat = Math.Min(gameHandler.players[enemy].creatureData.attack - 1, 2);
                    int stolenhp = Math.Min(gameHandler.players[enemy].creatureData.health, 2);

                    gameHandler.players[curPlayer].creatureData.attack += stolenat;
                    gameHandler.players[curPlayer].creatureData.health += stolenhp;

                    gameHandler.players[enemy].creatureData.attack -= stolenat;
                    gameHandler.players[enemy].creatureData.health -= stolenhp;

                    var info = extraInf as ExtraEffectInfo.AfterAttackInfo;

                    info.output.Add(
                        $"{gameHandler.players[curPlayer].name}'s Scrapbarber steals {stolenat}/{stolenhp} from {gameHandler.players[enemy].name}, " +
                        $"leaving it as a {gameHandler.players[enemy].creatureData.Stats()} and leaving {gameHandler.players[curPlayer].name} as a {gameHandler.players[curPlayer].creatureData.Stats()}.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class SuperScooper : Upgrade
        {
            public SuperScooper() :
                base(UpgradeSet.JunkAndTreasures, "Super Scooper", 8, 3, 7, Rarity.Rare, "Start of Combat: Steal the stats of the lowest-cost Upgrade your opponent bought last turn from their Mech.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Steal the stats of the lowest-cost Upgrade your opponent bought last turn from their Mech.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                    int lowestCost = 9999;
                    List<Upgrade> upgradesList = new List<Upgrade>();

                    for (int i = 0; i < gameHandler.players[enemy].buyHistory.Last().Count(); i++)
                    {
                        if (gameHandler.players[enemy].buyHistory.Last()[i].Cost < lowestCost && gameHandler.players[enemy].buyHistory.Last()[i].creatureData.attack != 0 && gameHandler.players[enemy].buyHistory.Last()[i].creatureData.health != 0)
                        {
                            lowestCost = gameHandler.players[enemy].buyHistory.Last()[i].Cost;
                            upgradesList.Clear();
                            upgradesList.Add(gameHandler.players[enemy].buyHistory.Last()[i]);
                        }
                    }

                    if (upgradesList.Count == 0)
                    {
                        info.output.Add(
                            $"{gameHandler.players[curPlayer].name}'s Super Scooper triggers but doesn't do anything.");
                        return Task.CompletedTask;
                    }

                    int pos = GameHandler.randomGenerator.Next(0, upgradesList.Count());

                    int attackst = upgradesList[pos].creatureData.attack;
                    int healthst = upgradesList[pos].creatureData.health;

                    upgradesList[pos].creatureData.attack = 0;
                    upgradesList[pos].creatureData.health = 0;

                    gameHandler.players[curPlayer].creatureData.attack += attackst;
                    gameHandler.players[curPlayer].creatureData.health += healthst;

                    gameHandler.players[enemy].creatureData.attack -= attackst;
                    gameHandler.players[enemy].creatureData.health -= healthst;

                    info.output.Add(
                        $"{gameHandler.players[curPlayer].name}'s Super Scooper steals {attackst}/{healthst} from {gameHandler.players[enemy].name}'s {upgradesList[pos].name}, " +
                        $"leaving it as a {gameHandler.players[enemy].creatureData.Stats()} and leaving {gameHandler.players[curPlayer].name} as a {gameHandler.players[curPlayer].creatureData.Stats()}.");

                    return Task.CompletedTask;
                }
            }
        }
    
        [Upgrade]
        public class CompetentScrapper : Upgrade
        {
            public CompetentScrapper() :
                base(UpgradeSet.JunkAndTreasures, "Competent Scrapper", 4, 3, 4, Rarity.Epic, "Battlecry: Discard all Spare Parts in your hand. Gain +3/+3 for each.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<int> handIndexes = gameHandler.players[curPlayer].hand.GetAllCardIndexes();

                    for (int i = 0; i < handIndexes.Count(); i++)
                    {
                        if (Attribute.IsDefined(gameHandler.players[curPlayer].hand.At(handIndexes[i]).GetType(), typeof(SparePartAttribute)))
                        {
                            gameHandler.players[curPlayer].hand.RemoveCard(handIndexes[i]);
                            gameHandler.players[curPlayer].creatureData.attack += 3;
                            gameHandler.players[curPlayer].creatureData.health += 3;
                        }
                    }

                    return Task.CompletedTask;
                }
            }
        }
    
        [Upgrade]
        public class FallenReaver : Upgrade
        {
            public FallenReaver() :
                base(UpgradeSet.JunkAndTreasures, "Fallen Reaver", 5, 8, 8, Rarity.Epic, "Aftermath: Destroy 6 random Upgrades in your shop.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Destroy 6 random Upgrades in your shop.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) break;

                        int index = gameHandler.players[curPlayer].shop.GetRandomUpgradeIndex();
                        Console.WriteLine(index);
                        gameHandler.players[curPlayer].shop.RemoveUpgrade(index);
                    }
                                        
                    gameHandler.players[curPlayer].aftermathMessages.Add("Your Fallen Reaver destroys 6 random Upgrades in your shop.");

                    return Task.CompletedTask;
                }
            }
        }
    
        [Upgrade]
        public class GarbageGrabber : Upgrade
        {
            public GarbageGrabber() :
                base(UpgradeSet.JunkAndTreasures, "Garbage Grabber", 5, 4, 4, Rarity.Epic, "Aftermath: Steal a random Common Upgrade from your opponent's shop and add it to yours.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: Steal a random Common Upgrade from your opponent's shop and add it to yours.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var enemyShopIndexes = gameHandler.players[enemy].shop.GetAllUpgradeIndexes();

                    var commonIndexes = CardsFilter.FilterList<int>(enemyShopIndexes, x => gameHandler.players[enemy].shop.At(x).rarity == Rarity.Common);

                    int stolen = commonIndexes[GameHandler.randomGenerator.Next(commonIndexes.Count)];

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Garbage Grabber stole a {gameHandler.players[enemy].shop.At(stolen).name} from your opponent's shop.");
                    gameHandler.players[enemy].aftermathMessages.Add(
                        $"{gameHandler.players[curPlayer].name}'s Garbage Grabber stole a Common Upgrade from your shop.");

                    gameHandler.players[curPlayer].shop.AddUpgrade(gameHandler.players[enemy].shop.At(stolen));
                    gameHandler.players[enemy].shop.RemoveUpgrade(stolen);

                    return Task.CompletedTask;
                }
            }
        }
    
        [Upgrade]
        public class GrandVault : Upgrade
        {
            public GrandVault() :
                base(UpgradeSet.JunkAndTreasures, "Grand Vault", 8, 7, 7, Rarity.Epic, "Permanent Aftermath: Add 3 random Upgrades to your shop.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Permanent Aftermath: Add 3 random Upgrades to your shop.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int poolSize = gameHandler.players[curPlayer].pool.upgrades.Count();
                        gameHandler.players[curPlayer].shop.AddUpgrade(gameHandler.players[curPlayer].pool.upgrades[GameHandler.randomGenerator.Next(0, poolSize)]);
                    }

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Grand Vault adds 3 random Upgrades to your shop.");

                    gameHandler.players[curPlayer].nextRoundEffects.Add(new Aftermath());

                    return Task.CompletedTask;
                }
            }
        }
    
        [Upgrade]
        public class Solartron3000 : Upgrade
        {
            public Solartron3000() :
                base(UpgradeSet.JunkAndTreasures, "Solartron 3000", 4, 2, 2, Rarity.Legendary, "Battlecry: The next Upgrade you buy this turn has Binary.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].effects.Add(new OnBuyingAMech());

                    return Task.CompletedTask;
                }
                private class OnBuyingAMech : Effect
                {
                    public OnBuyingAMech() : base(EffectType.OnBuyingAMech, "The next Upgrade you buy this turn has Binary.", EffectDisplayMode.Private) { }

                    public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        if (caller is Upgrade u)
                        {
                            u.creatureData.staticKeywords[StaticKeyword.Binary] = Math.Max(1, u.creatureData.staticKeywords[StaticKeyword.Binary]);
                            this._toBeRemoved = true;
                        }

                        return Task.CompletedTask;
                    }
                }
            }
        }

        [Upgrade]
        public class MrScrap4Cash : Upgrade
        {
            public MrScrap4Cash() : 
                base(UpgradeSet.JunkAndTreasures, "Mr. Scrap-4-Cash", 6, 6, 6, Rarity.Legendary, "Permanent Aftermath: Add a Receipt to your hand. It can refund your Upgrade's stats for Mana.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Permanent Aftermath: Add a Receipt to your hand. It can refund your Upgrade's stats for Mana.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].hand.AddCard(new Receipt());
                    gameHandler.players[curPlayer].nextRoundEffects.Add(new Aftermath());

                    return Task.CompletedTask;
                }
            }
            [Token]
            public class Receipt : Spell
            {
                public Receipt() :
                    base("Receipt", 0, "Name a number of Attack or Health. Remove that much from your Upgrade and gain half that Mana this turn only (rounded down).")
                {
                    this.effects.Add(new OnPlay());
                }
                private class OnPlay : Effect
                {
                    public OnPlay() : base(EffectType.OnPlay) { }

                    private bool InteractionCheck(string s, GameHandler gameHandler, ulong curPlayer)
                    {
                        var msg = s.Split();
                        if (msg.Count() != 2) return false;
                        if (!int.TryParse(msg[0], out int amount)) return false;
                        if (amount < 0) return false;

                        if (!(msg[1].Equals("attack", StringComparison.OrdinalIgnoreCase) || msg[1].Equals("health", StringComparison.OrdinalIgnoreCase))) return false;

                        if (msg[1].Equals("attack", StringComparison.OrdinalIgnoreCase))
                        {
                            if (gameHandler.players[curPlayer].creatureData.attack <= amount) return false;
                            gameHandler.players[curPlayer].creatureData.attack -= amount;
                        }
                        else if (msg[1].Equals("health", StringComparison.OrdinalIgnoreCase))
                        {
                            if (gameHandler.players[curPlayer].creatureData.health <= amount) return false;
                            gameHandler.players[curPlayer].creatureData.health -= amount;
                        }
                        else return false;

                        gameHandler.players[curPlayer].curMana += amount / 2;
                        return true;
                    }

                    public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        PlayerInteraction playerInteraction = new PlayerInteraction("Name a number of Attack or Health", "First type the number, followed by 'Attack' or 'Health'", "Capitalisation is ignored");
                        string defaultAns = "0 Attack";

                        await playerInteraction.SendInteractionAsync(gameHandler, curPlayer, this.InteractionCheck, defaultAns, extraInf.ctx);
                    }
                }
            }
        }       
    }
}
