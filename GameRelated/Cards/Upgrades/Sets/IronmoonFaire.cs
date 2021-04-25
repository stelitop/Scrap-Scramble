using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets
{
    public class IronmoonFaire
    {
        [Upgrade]
        public class ToyTank : Upgrade
        {
            public ToyTank() :
                base(UpgradeSet.IronmoonFaire, "Toy Tank", 1, 1, 3, Rarity.Common, "Taunt")
            {
                this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
            }
        }

        [Upgrade]
        public class ToyRocket : Upgrade
        {
            public ToyRocket() :
                base(UpgradeSet.IronmoonFaire, "Toy Rocket", 4, 3, 1, Rarity.Common, "Rush")
            {
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
            }
        }

        [Upgrade]
        public class ClawMachine : Upgrade
        {
            public ClawMachine() :
                base(UpgradeSet.IronmoonFaire, "Claw Machine", 3, 3, 2, Rarity.Common, "Battlecry: Add three 1/1 Plushies to your hand.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].hand.AddCard(new Plushie());
                    gameHandler.players[curPlayer].hand.AddCard(new Plushie());
                    gameHandler.players[curPlayer].hand.AddCard(new Plushie());

                    return Task.CompletedTask;
                }
            }

            [Token]
            public class Plushie : Upgrade
            {
                public Plushie() :
                    base(UpgradeSet.IronmoonFaire, "Plushie", 1, 1, 1, Rarity.Token, string.Empty)
                {

                }
            }
        }

        [Upgrade]
        public class MalfunctioningGuard : Upgrade
        {
            public MalfunctioningGuard() :
                base(UpgradeSet.IronmoonFaire, "Malfunctioning Guard", 4, 4, 8, Rarity.Common, "Start of Combat: Your Mech loses -4 Attack. Overload: (1)")
            {
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
                this.effects.Add(new StartOfCombat());
            }

            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Your Mech loses -4 Attack.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack -= 4;

                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                    info.output.Add(
                        $"{gameHandler.players[curPlayer].name}'s Malfunctioning Puncher reduces its Attack by 4, leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");


                    return Task.CompletedTask;
                }
            }
        }
    
        [Upgrade]
        public class FerrisWheel : Upgrade
        {
            public FerrisWheel() :
                base(UpgradeSet.IronmoonFaire, "Ferris Wheel", 6, 5, 5, Rarity.Common, "Aftermath: Return this to your shop. It costs (1) less than last time.")
            {
                this.effects.Add(new Aftermath());
            }

            private FerrisWheel(int reduction) :
                base(UpgradeSet.IronmoonFaire, "Ferris Wheel", 6, 5, 5, Rarity.Common, "Aftermath: Return this to your shop. It costs (1) less than last time.")
            {
                this.effects.Add(new Aftermath(reduction));
            }

            private class Aftermath : Effect
            {
                private int _reduction = 0;

                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Return this to your shop. It costs (1) less than last time.", EffectDisplayMode.Private) { }
                public Aftermath(int reduction) : base(EffectType.AftermathMe, "Aftermath: Return this to your shop. It costs (1) less than last time.", EffectDisplayMode.Private)
                {
                    this._reduction = reduction;
                }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var upgradeIndex = gameHandler.players[curPlayer].shop.AddUpgrade(new FerrisWheel(_reduction + 1));
                    gameHandler.players[curPlayer].shop.At(upgradeIndex).Cost -= _reduction + 1;
                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Ferris Wheel has returned to your shop. It now costs {gameHandler.players[curPlayer].shop.At(upgradeIndex).Cost}.");

                    return Task.CompletedTask;
                }

                public override Effect Copy()
                {
                    Aftermath ret = (Aftermath)base.Copy();
                    ret._reduction = this._reduction;
                    return ret;
                }
            }
        }

        [Upgrade]
        public class SwindlersCoin : Upgrade
        {
            public SwindlersCoin() : 
                base(UpgradeSet.IronmoonFaire, "Swindler's Coin", 1, 0, 1, Rarity.Common, "Binary, Tiebreaker. Overload: (1)")
            {
                this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
            }
        }

        [Upgrade]
        public class Highroller : Upgrade
        {
            public Highroller() :
                base(UpgradeSet.IronmoonFaire, "Highroller", 4, 3, 3, Rarity.Common, "Aftermath: Reduce the cost of a random Upgrade in your shop by (4).")
            {
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Reduce the cost of a random Upgrade in your shop by (4).", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return Task.CompletedTask;

                    Upgrade m = gameHandler.players[curPlayer].shop.GetRandomUpgrade();
                    m.Cost -= 4;

                    gameHandler.players[curPlayer].aftermathMessages.Add($"Your Highroller reduces the cost of {m.name} in your shop by (4).");

                    return Task.CompletedTask;
                }
            }
        }        

        [Upgrade]
        public class DjinniAccelerator : Upgrade
        {
            public DjinniAccelerator() :
                base(UpgradeSet.IronmoonFaire, "Djinni Accelerator", 5, 6, 6, Rarity.Common, "Magnetic, Taunt x2")
            {
                this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
                this.creatureData.staticKeywords[StaticKeyword.Taunt] = 2;
            }
        }
    
        [Upgrade]
        public class PeekABot : Upgrade
        {
            public PeekABot() :
                base(UpgradeSet.IronmoonFaire, "Peek-a-Bot", 1, 1, 1, Rarity.Rare, "Aftermath: You are told the most expensive Upgrade in your opponent's shop.")
            {
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: You are told the most expensive Upgrade in your opponent's shop.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (curPlayer == enemy) return Task.CompletedTask;
                    if (gameHandler.players[enemy].shop.OptionsCount() == 0)
                    {
                        gameHandler.players[curPlayer].aftermathMessages.Add(
                            "Your opponent's shop is empty.");
                        return Task.CompletedTask;
                    }
                    List<Upgrade> enemyMechs = gameHandler.players[enemy].shop.GetAllUpgrades();
                    List<int> highestCosts = new List<int>();
                    int maxCost = -1;

                    for (int i = 0; i < enemyMechs.Count(); i++)
                    {
                        if (maxCost < enemyMechs[i].Cost) maxCost = enemyMechs[i].Cost;
                    }

                    for (int i = 0; i < enemyMechs.Count(); i++)
                    {
                        if (enemyMechs[i].Cost == maxCost) highestCosts.Add(i);
                    }

                    int pos = GameHandler.randomGenerator.Next(0, highestCosts.Count());

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Peek-a-Bot tells you the most expensive Upgrade in your opponent's shop is {enemyMechs[highestCosts[pos]].name}");
                    return Task.CompletedTask;
                }
            }
        }
    
        [Upgrade]
        public class PrizeStacker : Upgrade
        {
            public PrizeStacker() :
                base(UpgradeSet.IronmoonFaire, "Prize Stacker", 4, 2, 4, Rarity.Rare, "Battlecry: Give your Mech +1/+1 for each card in your hand.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].hand.GetAllCardIndexes().Count();
                    gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].hand.GetAllCardIndexes().Count();
                    return Task.CompletedTask;
                }
            }

        }
    
        [Upgrade]
        public class RoboRabbit : Upgrade
        {
            public RoboRabbit() :
                base(UpgradeSet.IronmoonFaire, "Robo-Rabbit", 2, 1, 1, Rarity.Rare, "Battlecry: Gain +2/+2 for each other Robo-Rabbit you've played this game.")
            {
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    int rabbits = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, x => x.name == "Robo-Rabbit").Count();

                    gameHandler.players[curPlayer].creatureData.attack += 2;
                    gameHandler.players[curPlayer].creatureData.health += 2;

                    return Task.CompletedTask;
                }
            }

            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                return base.GetInfo(gameHandler, player) + $" *({CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, x => x.name == "Robo-Rabbit").Count})*";
            }
        }           

        [Upgrade]
        public class BumperCar : Upgrade
        {
            public BumperCar() :
                base(UpgradeSet.IronmoonFaire, "Bumper Car", 3, 5, 1, Rarity.Rare, "Rush x2. After this takes damage, destroy it.")
            {
                this.effects.Add(new OnTakingDamage());
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 2;
            }
            private class OnTakingDamage : Effect
            {
                public OnTakingDamage() : base(EffectType.AfterThisTakesDamage, "After this takes damage, destroy it.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].destroyed = true;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class LightningWeasel : Upgrade
        {
            public LightningWeasel() :
                base(UpgradeSet.IronmoonFaire, "Lightning Weasel", 2, 1, 1, Rarity.Rare, "Aftermath: Replace the highest-Cost Upgrade in your opponent's shop with a Lightning Weasel.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: Replace the highest-Cost Upgrade in your opponent's shop with a Lightning Weasel.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (curPlayer == enemy) return Task.CompletedTask;
                    if (gameHandler.players[enemy].shop.OptionsCount() == 0) return Task.CompletedTask;

                    List<int> highestCosts = new List<int>();
                    List<int> enemyIndexes = gameHandler.players[enemy].shop.GetAllUpgradeIndexes();

                    int maxCost = -1;
                    for (int i = 0; i < enemyIndexes.Count; i++)
                    {
                        if (maxCost < gameHandler.players[enemy].shop.At(enemyIndexes[i]).Cost)
                            maxCost = gameHandler.players[enemy].shop.At(enemyIndexes[i]).Cost;
                    }

                    for (int i = 0; i < enemyIndexes.Count(); i++)
                    {
                        if (gameHandler.players[enemy].shop.At(enemyIndexes[i]).Cost == maxCost) highestCosts.Add(i);
                    }

                    int pos = GameHandler.randomGenerator.Next(0, highestCosts.Count());

                    string oldName = gameHandler.players[enemy].shop.At(highestCosts[pos]).name;
                    gameHandler.players[enemy].shop.TransformUpgrade(highestCosts[pos], new LightningWeasel());

                    gameHandler.players[enemy].aftermathMessages.Add(
                        $"{gameHandler.players[curPlayer].name}'s Lightning Weasel replaced your highest-cost Upgrade ({oldName}) with a Lightning Weasel.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class RaffleBox : Upgrade
        {
            public RaffleBox() :
                base(UpgradeSet.IronmoonFaire, "Raffle Box", 4, 3, 4, Rarity.Rare, "Aftermath: Add an Ironmoon Ticket to your hand. It gives you +1/+1 and 1 Mana for each Ticket you're holding.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Add an Ironmoon Ticket to your hand.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].hand.AddCard(new IronmoonTicket());
                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Raffle Box adds an Ironmoon Ticket to your hand.");

                    return Task.CompletedTask;
                }
            }

        }

        //[Upgrade]
        //public class Gamblebot : Upgrade
        //{
        //    public Gamblebot() :
        //        base(UpgradeSet.IronmoonFaire, "Gamblebot", 3, 2, 3, Rarity.Rare, "Aftermath: Give the winner of your last combat +2 Mana this turn only.")
        //    {
        //        this.effects.Add(new Aftermath());
        //    }
        //    private class Aftermath : Effect
        //    {
        //        public Aftermath() : base(new EffectType[] { EffectType.AftermathMe, EffectType.AftermathEnemy }, "Aftermath: Give the winner of your last combat +2 Coins.", EffectDisplayMode.Private) { }

        //        public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
        //        {
        //            if (extraInf.calledEffect == EffectType.AftermathMe)
        //            {
        //                if (gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count()-2][curPlayer] == FightResult.WIN)
        //                {
        //                    gameHandler.players[curPlayer].curMana += 2;
        //                    gameHandler.players[curPlayer].aftermathMessages.Add(
        //                        "Your Gamblebot gives you +2 Mana, because you won last combat.");
        //                }
        //                else
        //                {
        //                    gameHandler.players[curPlayer].aftermathMessages.Add(
        //                        "Your Gamblebot fails to trigger, because you lost last combat.");
        //                }
        //            }
        //            else if (extraInf.calledEffect == EffectType.AftermathEnemy)
        //            {                        
        //                if (gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count() - 2][enemy] == FightResult.WIN)
        //                {
        //                    gameHandler.players[enemy].curMana += 2;
        //                    gameHandler.players[enemy].aftermathMessages.Add(
        //                        $"{gameHandler.players[curPlayer].name}'s Gamblebot gives you +2 Mana, because you won last combat.");
        //                }
        //            }

        //            return Task.CompletedTask;
        //        }
        //    }
        //}

        [Upgrade]
        public class SpringloadedJester : Upgrade
        {
            public SpringloadedJester() :
                base(UpgradeSet.IronmoonFaire, "Springloaded Jester", 2, 1, 1, Rarity.Epic, "After this attacks, swap your Mech's Attack and Health.")
            {
                this.effects.Add(new AfterThisAttacks());
            }

            private class AfterThisAttacks : Effect
            {
                public AfterThisAttacks() : base(EffectType.AfterThisTakesDamage, "After this attacks, swap your Mech's Attack and Health.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    int mid = gameHandler.players[curPlayer].creatureData.attack;
                    gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
                    gameHandler.players[curPlayer].creatureData.health = mid;

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class FortuneWheel : Upgrade
        {
            public FortuneWheel() :
                base(UpgradeSet.IronmoonFaire, "Fortune Wheel", 3, 3, 3, Rarity.Epic, "Aftermath: Cast 3 random Spare Parts with random targets.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Cast 3 random Spare Parts on random Upgrades in your shop.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    string aftermathMsg = "Your Fortune Wheel casted ";

                    for (int i = 0; i < 3; i++)
                    {
                        Spell sparePart = (Spell)gameHandler.players[curPlayer].pool.spareparts[GameHandler.randomGenerator.Next(0, gameHandler.pool.spareparts.Count())].DeepCopy();

                        if (i == 0) aftermathMsg += $"{sparePart.name}";
                        else if (i == 1) aftermathMsg += $", {sparePart.name}";
                        else if (i == 2) aftermathMsg += $" and {sparePart.name}";

                        if (sparePart.name == "Mana Capsule")
                        {
                            //await sparePart.OnPlay(gameHandler, curPlayer, enemy);
                            gameHandler.players[curPlayer].curMana += 2;
                        }
                        else
                        {
                            int index = gameHandler.players[curPlayer].shop.GetRandomUpgradeIndex();
                            sparePart.CastOnUpgradeInShop(index, gameHandler, curPlayer, enemy, extraInf.ctx);
                            aftermathMsg += $"({gameHandler.players[curPlayer].shop.At(index).name})";
                        }
                    }

                    aftermathMsg += " on random Upgrades in your shop.";

                    gameHandler.players[curPlayer].aftermathMessages.Add(aftermathMsg);

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class TrickRoomster : Upgrade
        {
            public TrickRoomster() :
                base(UpgradeSet.IronmoonFaire, "Trick Roomster", 4, 1, 1, Rarity.Epic, "The Mech with the lower Attack Priority goes first instead.")
            {
                this.effects.Add(new OnPlay());
            }

            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay, "The Mech with the lower Attack Priority goes first instead.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].specificEffects.invertAttackPriority = true;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class Mirrordome : Upgrade
        {
            public Mirrordome() :
                base(UpgradeSet.IronmoonFaire, "Mirrordome", 4, 0, 8, Rarity.Epic, "Aftermath: This turn, your shop is a copy of your opponent's.")
            {
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: This turn, your shop is a copy of your opponent's.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (curPlayer == enemy) return Task.CompletedTask;

                    gameHandler.players[curPlayer].shop.Clear();

                    for (int i = 0; i < gameHandler.players[enemy].shop.LastIndex; i++)
                    {
                        gameHandler.players[curPlayer].shop.AddUpgrade(gameHandler.players[enemy].shop.At(i));
                    }

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Mirrordome replaced your shop with a copy of {gameHandler.players[enemy].name}'s shop.");

                    return Task.CompletedTask;
                }
            }
        }    
    
        [Upgrade]
        public class HatChucker8000 : Upgrade
        {
            public HatChucker8000() :
                base(UpgradeSet.IronmoonFaire, "Hat Chucker 8000", 3, 3, 3, Rarity.Legendary, "Battlecry: Name a Rarity. Aftermath: Give all players' Upgrades of that rarity +2/+2.")
            {
                this.effects.Add(new Battlecry());                
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var prompt = new PlayerInteraction("Name a Rarity", "Common, Rare, Epic or Legendary", "Capitalisation is ignored");
                    List<string> rarities = new List<string>() { "common", "rare", "epic", "legendary" };
                    string defaultAns = rarities[GameHandler.randomGenerator.Next(4)];

                    string ret = await prompt.SendInteractionAsync(gameHandler, curPlayer, (x, y, z) => rarities.Contains(x.ToLower()), defaultAns, extraInf.ctx);

                    Rarity chosenRarity;

                    if (ret.Equals("common", StringComparison.OrdinalIgnoreCase)) chosenRarity = Rarity.Common;
                    else if (ret.Equals("rare", StringComparison.OrdinalIgnoreCase)) chosenRarity = Rarity.Rare;
                    else if (ret.Equals("epic", StringComparison.OrdinalIgnoreCase)) chosenRarity = Rarity.Epic;
                    else if (ret.Equals("legendary", StringComparison.OrdinalIgnoreCase)) chosenRarity = Rarity.Legendary;
                    else chosenRarity = Rarity.NO_RARITY;

                    //this.writtenEffect = $"Aftermath: Give all players' {this.chosenRarity} Upgrades in their shops +2/+2.";

                    Aftermath effect = new Aftermath(chosenRarity);
                    effect.effectText = $"Aftermath: Give all players' {chosenRarity} Upgrades in their shops +2/+2.";
                    effect.displayMode = EffectDisplayMode.Private;

                    gameHandler.players[curPlayer].effects.Add(effect);
                }

                private class Aftermath : Effect
                {
                    private Rarity _rarity;
                    public Aftermath() : base(EffectType.AftermathEnemy) { }
                    public Aftermath(Rarity rarity) : base(EffectType.AftermathEnemy) { _rarity = rarity; }
                    public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        List<Upgrade> upgrades = gameHandler.players[curPlayer].shop.GetAllUpgrades();

                        for (int i = 0; i < upgrades.Count(); i++)
                        {
                            if (upgrades[i].rarity == _rarity)
                            {
                                upgrades[i].creatureData.attack += 2;
                                upgrades[i].creatureData.health += 2;
                            }
                        }

                        gameHandler.players[curPlayer].aftermathMessages.Add(
                            $"Your Hat Chucker 8000 gave your {_rarity} Upgrades +2/+2.");

                        foreach (var player in gameHandler.players)
                        {
                            if (player.Key == curPlayer) continue;
                            if (player.Value.lives <= 0) continue;

                            upgrades = player.Value.shop.GetAllUpgrades();

                            for (int i = 0; i < upgrades.Count(); i++)
                            {
                                if (upgrades[i].rarity == _rarity)
                                {
                                    upgrades[i].creatureData.attack += 2;
                                    upgrades[i].creatureData.health += 2;
                                }
                            }

                            player.Value.aftermathMessages.Add(
                                $"{gameHandler.players[curPlayer].name}'s Hat Chucker 8000 gave your {_rarity} Upgrades +2/+2.");
                        }

                        return Task.CompletedTask;
                    }
                }
            }
        }
    
        [Upgrade]
        public class SilasIronmoon : Upgrade
        {
            public SilasIronmoon() :
                base(UpgradeSet.IronmoonFaire, "Silas Ironmoon", 7, 4, 4, Rarity.Legendary, "Permanent Aftermath: Add an Ironmoon Ticket to your hand. It gives you +1/+1 and 1 Mana for each Ticket you're holding.")
            {
                this.effects.Add(new PermanentAftermath());
            }

            private class PermanentAftermath : Effect
            {
                public PermanentAftermath() : base(EffectType.AftermathMe, "Permanent Aftermath: Add an Ironmoon Ticket to your hand.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].hand.AddCard(new IronmoonTicket());
                    gameHandler.players[curPlayer].nextRoundEffects.Add(new PermanentAftermath());

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Silas Ironmoon adds an Ironmoon Ticket to your hand.");

                    return Task.CompletedTask;
                }
            }            
        }

        [Token]
        public class IronmoonTicket : Spell
        {
            public IronmoonTicket() :
                base("Ironmoon Ticket", 0, "Gain +1/+1 and 1 Mana for each Ironmoon Ticket you're holding.")
            {
                this.effects.Add(new OnPlay());
            }

            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    int amountOfTickets = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].hand.GetAllCards(), x => x.name == "Ironmoon Ticket").Count();

                    gameHandler.players[curPlayer].creatureData.attack += amountOfTickets + 1;
                    gameHandler.players[curPlayer].creatureData.health += amountOfTickets + 1;
                    gameHandler.players[curPlayer].curMana += amountOfTickets + 1;

                    return Task.CompletedTask;
                }
            }

            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                List<Card> amountOfTickets = CardsFilter.FilterList<Card>(gameHandler.players[player].hand.GetAllCards(), x => x.name == "Ironmoon Ticket");

                return base.GetInfo(gameHandler, player) + $" *({amountOfTickets.Count()})*";
            }
        }
    }
}
