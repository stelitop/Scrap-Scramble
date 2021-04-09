﻿using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets
{
    public class MonstersReanimated
    {
        [Upgrade]
        public class GoldWingerGryphin : Upgrade
        {
            public GoldWingerGryphin() :
                base(UpgradeSet.MonstersReanimated, "Gold-Winged Gryphin", 5, 4, 1, Rarity.Common, "Battlecry: Discover a Rush Upgrade.")
            {
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
                this.effects.Add(new Battlecry());
            }

            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Upgrade> rushUpgrades = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades,
                        x => x.creatureData.staticKeywords[StaticKeyword.Rush] > 0 && x.name != "Gold-Winger Gryphin");

                    await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, extraInf.ctx, "Discover a Rush Upgrade", rushUpgrades);
                }
            }
        }

        [Upgrade]
        public class ThreeModuleHydra : Upgrade
        {
            public ThreeModuleHydra() :
                base(UpgradeSet.MonstersReanimated, "Three-Module Hydra", 6, 6, 6, Rarity.Rare, "Aftermath: Add a 1/1, 2/2 and 3/3 Head Module to your hand.")
            {
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Add a 1/1, 2/2 and 3/3 Head Module to your hand.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].hand.AddCard(new LeftModuleHead());
                    gameHandler.players[curPlayer].hand.AddCard(new CenterModuleHead());
                    gameHandler.players[curPlayer].hand.AddCard(new RightModuleHead());

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Three-Module Hydra adds a 1/1, 2/2 and 3/3 Head Module to your hand.");

                    return Task.CompletedTask;
                }
            }

            [Token]
            public class LeftModuleHead : Upgrade
            {
                public LeftModuleHead() :
                    base(UpgradeSet.None, "Left Module Head", 1, 1, 1, Rarity.NO_RARITY, string.Empty)
                { }
            }
            [Token]
            public class RightModuleHead : Upgrade
            {
                public RightModuleHead() :
                    base(UpgradeSet.None, "Right Module Head", 2, 2, 2, Rarity.NO_RARITY, string.Empty)
                { }
            }
            [Token]
            public class CenterModuleHead : Upgrade
            {
                public CenterModuleHead() :
                    base(UpgradeSet.None, "Center Module Head", 3, 3, 3, Rarity.NO_RARITY, string.Empty)
                { }
            }
        }

        [Upgrade]
        public class MkIVSuperCobra : Upgrade
        {
            public MkIVSuperCobra() :
                base(UpgradeSet.MonstersReanimated, "Mk. IV Super Cobra", 6, 5, 2, Rarity.Rare, "Rush. Aftermath: Destroy a random Upgrade in your opponent's shop.")
            {
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: Destroy a random Upgrade in your opponent's shop.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (curPlayer == enemy) return Task.CompletedTask;

                    if (gameHandler.players[enemy].shop.OptionsCount() == 0) return Task.CompletedTask;

                    int index = gameHandler.players[enemy].shop.GetRandomUpgradeIndex();
                    gameHandler.players[enemy].shop.RemoveUpgrade(index);

                    gameHandler.players[enemy].aftermathMessages.Add($"{gameHandler.players[curPlayer].name}'s Mk. IV Super Cobra destroyed a random upgrade in your shop.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class DungeonDragonling : Upgrade
        {
            public DungeonDragonling() :
                base(UpgradeSet.MonstersReanimated, "Dungeon Dragonling", 12, 4, 12, Rarity.Epic, "Whenever you would take damage, roll a d20. You take that much less damage.")
            {
                this.effects.Add(new BeforeTakingDamage());
            }

            private class BeforeTakingDamage : Effect
            {
                public BeforeTakingDamage() : base(EffectType.BeforeTakingDamage, "Whenever you would take damage, roll a d20. You take that much less damage.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (extraInf is ExtraEffectInfo.DamageInfo attackInf)
                    {
                        int red = GameHandler.randomGenerator.Next(1, 21);
                        attackInf.dmg -= red;
                        if (attackInf.dmg < 0) attackInf.dmg = 0;

                        attackInf.output += $"reduced to {attackInf.dmg} by Dungeon Dragonling(rolled {red}), ";
                    }

                    return Task.CompletedTask;
                }
            }

        }

        [Upgrade]
        public class LadyInByte : Upgrade
        {
            public LadyInByte() :
                base(UpgradeSet.MonstersReanimated, "Lady in Byte", 7, 5, 5, Rarity.Legendary, "Aftermath: Set your Mech's Attack equal to its Health.")
            {
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Set your Mech's Attack equal to its Health.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Lady in Byte set your Attack equal to your Health ({gameHandler.players[curPlayer].creatureData.Stats()})");
                    return Task.CompletedTask;
                }
            }
        }

        [Token]
        public class Mechathun : Upgrade
        {
            public Mechathun() : 
                base(UpgradeSet.MonstersReanimated, "Mecha'thun", 0, 50, 50, Rarity.Token, string.Empty)
            {
                this.creatureData.staticKeywords[StaticKeyword.Freeze] = 10;
            }

            public override string GetInfo(GameHandler gameHandler, ulong player)
            {
                string ret = string.Empty;
                if (this.cardText.Equals(string.Empty)) ret = $"{this.name} - {this.rarity} - {this.Cost}/{this.creatureData.attack}/{this.creatureData.health}";
                else ret = $"{this.name} - {this.rarity} - {this.Cost}/{this.creatureData.attack}/{this.creatureData.health} - {this.cardText}";

                if (this.creatureData.staticKeywords[StaticKeyword.Freeze] == 1) ret = $"(Thaws in 1 turn) {ret}";
                else if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 1) ret = $"(Thaws in {this.creatureData.staticKeywords[StaticKeyword.Freeze]} turns) {ret}";
                return ret;
            }

            public override Upgrade BasicCopy(CardPool pool)
            {
                Upgrade ret = base.BasicCopy(pool);
                ret.creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
                return ret;
            }

            /// <summary>
            /// Checks whether ot not Mecha'thun is in a player's shop. Returns the index of the spot if yes. Returns -1 otherwise.
            /// </summary>
            /// <param name="gameHandler"></param>
            /// <param name="player"></param>
            /// <returns></returns>
            public static int FindInShop(GameHandler gameHandler, ulong player)
            {
                int ret = -1;

                for (int i = 0; i < gameHandler.players[player].shop.LastIndex; i++)
                {
                    if (gameHandler.players[player].shop.At(i).name.Equals("Mecha'thun"))
                    {
                        ret = i;
                        break;
                    }
                }

                return ret;
            }

            public static int AddMechaThun(GameHandler gameHandler, ulong player)
            {
                Card token = new Mechathun();
                return gameHandler.players[player].shop.AddUpgrade((Upgrade)gameHandler.players[player].pool.FindBasicCard(token.name));
            }
        }

        private class GenerateMechathunEffect : Effect
        {
            public GenerateMechathunEffect() : base(EffectType.OnPlay) { }            

            public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
            {
                int index = Mechathun.FindInShop(gameHandler, curPlayer);
                if (index == -1) Mechathun.AddMechaThun(gameHandler, curPlayer);

                return Task.CompletedTask;
            }
        }

        [Upgrade]
        public class MechathunsSeeker : Upgrade
        {
            public MechathunsSeeker() :
                base(UpgradeSet.MonstersReanimated, "Mecha'thun's Seeker", 2, 1, 2, Rarity.Common, "Battlecry: Your Mecha'thun thaws 1 turn sooner.")
            {
                this.effects.Add(new GenerateMechathunEffect());
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var mechathun = gameHandler.players[curPlayer].shop.At(Mechathun.FindInShop(gameHandler, curPlayer));

                    mechathun.creatureData.staticKeywords[StaticKeyword.Freeze]--;
                    if (mechathun.creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                        mechathun.creatureData.staticKeywords[StaticKeyword.Freeze] = 0;

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class MechathunsSlayer : Upgrade
        {
            public MechathunsSlayer() :
                base(UpgradeSet.MonstersReanimated, "Mecha'thun's Slayer", 6, 3, 4, Rarity.Common, "Rush. Battlecry: Your Mecha'thun thaws 1 turn sooner.")
            {
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
                this.effects.Add(new GenerateMechathunEffect());
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var mechathun = gameHandler.players[curPlayer].shop.At(Mechathun.FindInShop(gameHandler, curPlayer));

                    mechathun.creatureData.staticKeywords[StaticKeyword.Freeze]--;
                    if (mechathun.creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                        mechathun.creatureData.staticKeywords[StaticKeyword.Freeze] = 0;

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class MechathunsHarbinger : Upgrade
        {
            public MechathunsHarbinger() :
                base(UpgradeSet.MonstersReanimated, "Mecha'thun's Harbinger", 10, 9, 9, Rarity.Common, "Taunt. Battlecry: Your Mecha'thun thaws 2 turns sooner.")
            {
                this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
                this.effects.Add(new GenerateMechathunEffect());
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var mechathun = gameHandler.players[curPlayer].shop.At(Mechathun.FindInShop(gameHandler, curPlayer));

                    mechathun.creatureData.staticKeywords[StaticKeyword.Freeze]-=2;
                    if (mechathun.creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                        mechathun.creatureData.staticKeywords[StaticKeyword.Freeze] = 0;

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class MechathunsElder : Upgrade
        {
            public MechathunsElder() :
                base(UpgradeSet.MonstersReanimated, "Mecha'thun's Elder", 4, 12, 12, Rarity.Rare, "Battlecry: Give your Mech -1/-1 for each turn your Mecha'thun has left to thaw.")
            {
                this.effects.Add(new GenerateMechathunEffect());
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var mechathun = gameHandler.players[curPlayer].shop.At(Mechathun.FindInShop(gameHandler, curPlayer));

                    gameHandler.players[curPlayer].creatureData.attack -= mechathun.creatureData.staticKeywords[StaticKeyword.Freeze];
                    gameHandler.players[curPlayer].creatureData.health -= mechathun.creatureData.staticKeywords[StaticKeyword.Freeze];

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class MechathunsLynchpin : Upgrade
        {
            public MechathunsLynchpin() :
                base(UpgradeSet.MonstersReanimated, "Mecha'thun's Lynchpin", 8, 8, 8, Rarity.Rare, "Battlecry: If your Mecha'thun has 5 or fewer turns left to thaw, gain +16 Shields.")
            {
                this.effects.Add(new GenerateMechathunEffect());
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var mechathun = gameHandler.players[curPlayer].shop.At(Mechathun.FindInShop(gameHandler, curPlayer));

                    if (mechathun.creatureData.staticKeywords[StaticKeyword.Freeze] <= 5)
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 16;

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class MechathunsGenerator : Upgrade
        {
            public MechathunsGenerator() :
                base(UpgradeSet.MonstersReanimated, "Mecha'thun's Generator", 5, 2, 5, Rarity.Rare, "Aftermath: Add 3 other random Mecha'thun Cultists to your shop.")
            {
                this.effects.Add(new GenerateMechathunEffect());
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Add 3 random Mecha'thun Cultists to your shop.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<Upgrade> list = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.name.Contains("Mecha'thun") && !x.name.Equals("Mecha'thun's Generator"));

                    for (int i = 0; i < 3; i++)
                    {
                        int card = GameHandler.randomGenerator.Next(0, list.Count());

                        gameHandler.players[curPlayer].shop.AddUpgrade(list[card]);
                    }

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Mecha'thun's Generator adds 3 other random Mecha'thun Cultists to your shop.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class MechathunsLiege : Upgrade
        {
            public MechathunsLiege() :
                base(UpgradeSet.MonstersReanimated, "Mecha'thun's Liege", 12, 12, 12, Rarity.Common, "Battlecry: Your Mecha'thun thaws 3 turns sooner. Overload: (3)")
            {
                this.effects.Add(new GenerateMechathunEffect());
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var mechathun = gameHandler.players[curPlayer].shop.At(Mechathun.FindInShop(gameHandler, curPlayer));

                    mechathun.creatureData.staticKeywords[StaticKeyword.Freeze]-=3;
                    if (mechathun.creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                        mechathun.creatureData.staticKeywords[StaticKeyword.Freeze] = 0;

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class MechathunsLord : Upgrade
        {
            public MechathunsLord() :
                base(UpgradeSet.MonstersReanimated, "Mecha'thun's Lord", 10, 10, 10, Rarity.Epic, "Spellburst: Give your Mecha'thun +20/+20.")
            {
                this.effects.Add(new GenerateMechathunEffect());
                this.effects.Add(new Spellburst());
            }
            private class Spellburst : Effect
            {
                public Spellburst() : base(EffectType.Spellburst, "Spellburst: Give your Mecha'thun +20/+20.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var mechathun = gameHandler.players[curPlayer].shop.At(Mechathun.FindInShop(gameHandler, curPlayer));

                    mechathun.creatureData.attack += 20;
                    mechathun.creatureData.health += 20;

                    return Task.CompletedTask;
                }
            }
        }
    }
}