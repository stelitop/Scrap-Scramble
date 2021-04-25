using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets
{
    public class ScholomanceAcademy
    {
        [Upgrade]
        public class OneHitWonder : Upgrade
        {
            public OneHitWonder() :
                base(UpgradeSet.ScholomanceAcademy, "One Hit Wonder", 5, 1, 5, Rarity.Common, "Start of Combat: Gain +8 Attack.")
            {
                this.effects.Add(new StartOfCombat());
            }

            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Gain +8 Attack", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack += 8;

                    if (extraInf is ExtraEffectInfo.StartOfCombatInfo info)
                    {
                        info.output.Add($"{gameHandler.players[curPlayer].name}'s One Hit Wonder gives it +8 Attack, leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class SurveillanceBird : Upgrade
        {
            public SurveillanceBird() :
                base(UpgradeSet.ScholomanceAcademy, "Surveillance Bird", 3, 2, 2, Rarity.Common, "Aftermath: Gain 2 Mana this turn only.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Gain 2 Mana this turn only.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].curMana += 2;
                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Surveillance Bird gives you +2 Mana this turn only.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class OnyxCrowbot : Upgrade
        {
            public OnyxCrowbot() :
                base(UpgradeSet.ScholomanceAcademy, "Onyx Crowbot", 5, 4, 4, Rarity.Common, "Aftermath: Gain 4 Mana this turn only.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Gain 4 Mana this turn only.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].curMana += 4;
                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Onyx Crowbot gives you +4 Mana this turn only.");
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class SulfurNanoPhoenix : Upgrade
        {
            public SulfurNanoPhoenix() :
                base(UpgradeSet.ScholomanceAcademy, "Sulfur Nano-Phoenix", 7, 6, 6, Rarity.Common, "Aftermath: Gain 6 Mana this turn only.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Gain 6 Mana this turn only.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].curMana += 6;
                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        $"Your Sulfur Nano-Phoenix gives you +6 Mana this turn only.");
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class Tinkerpet : Upgrade
        {
            public Tinkerpet() :
                base(UpgradeSet.ScholomanceAcademy, "Tinkerpet", 2, 1, 1, Rarity.Common, "Spellburst: Gain +4 Spikes and +4 Shields.")
            {
                this.effects.Add(new Spellburst());
            }
            private class Spellburst : Effect
            {
                public Spellburst() : base(EffectType.Spellburst, "Spellburst: Gain +4 Spikes and +4 Shields.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class PrismaticBarrier : Upgrade
        {
            public PrismaticBarrier() :
                base(UpgradeSet.ScholomanceAcademy, "Prismatic Barrier", 8, 5, 8, Rarity.Common, "Start of Combat: Gain +10 Shields.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: Gain +10 Shields.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var info = extraInf as ExtraEffectInfo.StartOfCombatInfo;

                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 10;

                    info.output.Add($"{gameHandler.players[curPlayer].name}'s Prismatic Barrier gives it +10 Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class ThreeFacedEmojitron : Upgrade
        {
            public ThreeFacedEmojitron() :
                base(UpgradeSet.ScholomanceAcademy, "Three-Faced Emojitron", 5, 4, 2, Rarity.Common, "Choose One - Gain Rush; or +2/+2.")
            {
                this.effects.Add(new ChooseOne());
            }

            private class ChooseOne : Effect
            {
                public ChooseOne() : base(EffectType.OnPlay) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    PlayerInteraction chooseOne = new PlayerInteraction("Choose One", "1) Gain +2/+2\n2) Gain Rush", "Write the corresponding number");
                    string ret = await chooseOne.SendInteractionAsync(gameHandler, curPlayer, (x, y, z) => GeneralFunctions.Within(x, 1, 2), GameHandler.randomGenerator.Next(1, 3).ToString(), extraInf.ctx);

                    int choice = int.Parse(ret);

                    if (choice == 1)
                    {
                        gameHandler.players[curPlayer].creatureData.attack += 2;
                        gameHandler.players[curPlayer].creatureData.health += 2;
                    }
                    else if (choice == 2)
                    {
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush]++;
                    }
                }
            }
        }

        [Upgrade]
        public class RivetedTrinked : Upgrade
        {
            public RivetedTrinked() :
                base(UpgradeSet.ScholomanceAcademy, "Riveted Trinked", 2, 1, 1, Rarity.Common, "Binary. Battlecry: Gain +2 Shields.")
            {
                this.effects.Add(new Battlecry());
                this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
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
        }

        [Upgrade]
        public class ArcaneAutomatron : Upgrade
        {
            public ArcaneAutomatron() : 
                base(UpgradeSet.ScholomanceAcademy, "Arcane Automatron", 2, 1, 3, Rarity.Rare, "Buying this Upgrade also counts as casting a spell.")
            {
                this.effects.Add(new OnPlay());
            }
            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    await Effect.CallEffects(gameHandler.players[curPlayer].effects, Effect.EffectType.OnSpellCast, caller, gameHandler, curPlayer, enemy, extraInf);
                    await Effect.CallEffects(gameHandler.players[curPlayer].effects, Effect.EffectType.Spellburst, caller, gameHandler, curPlayer, enemy, extraInf, true);
                }
            }
        }

        [Upgrade]
        public class CobaltConqueror : Upgrade
        {
            public CobaltConqueror() : 
                base(UpgradeSet.ScholomanceAcademy, "Cobalt Conqueror", 10, 9, 7, Rarity.Rare, "Rush. Combo: Give the next Upgrade you buy this turn Rush.")
            {
                this.effects.Add(new Combo());
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
            }
            private class Combo : Effect
            {
                public Combo() : base(EffectType.Combo) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].effects.Add(new OnBuyingAMech());
                    return Task.CompletedTask;
                }
                private class OnBuyingAMech : Effect
                {
                    public OnBuyingAMech() : base(EffectType.OnBuyingAMech, "Give the next Upgrade you buy this turn Rush.", EffectDisplayMode.Private) { }

                    public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        if (caller is Upgrade u)
                        {
                            u.creatureData.staticKeywords[StaticKeyword.Rush]++;
                            this._toBeRemoved = true;
                        }

                        return Task.CompletedTask;
                    }
                }
            }
        }

        [Upgrade]
        public class LivewireBramble : Upgrade
        {
            public LivewireBramble() :
                base(UpgradeSet.ScholomanceAcademy, "Livewire Bramble", 0, 2, 1, Rarity.Rare, "Aftermath: Replace two random Upgrades in your shop with Livewire Brambles.")
            {
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Replace two random Upgrades in your shop with Livewire Brambles.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<int> upgrades = gameHandler.players[curPlayer].shop.GetAllUpgradeIndexes();

                    if (upgrades.Count() <= 2)
                    {
                        for (int i = 0; i < upgrades.Count(); i++)
                        {
                            gameHandler.players[curPlayer].shop.TransformUpgrade(upgrades[i], new LivewireBramble());
                        }
                    }
                    else
                    {
                        int pos1, pos2;
                        pos1 = GameHandler.randomGenerator.Next(0, upgrades.Count());
                        pos2 = GameHandler.randomGenerator.Next(0, upgrades.Count() - 1);
                        if (pos2 >= pos1) pos2++;

                        gameHandler.players[curPlayer].shop.TransformUpgrade(upgrades[pos1], new LivewireBramble());
                        gameHandler.players[curPlayer].shop.TransformUpgrade(upgrades[pos2], new LivewireBramble());
                    }

                    gameHandler.players[curPlayer].aftermathMessages.Add(
                        "Your Livewire Bramble replace two Upgrades in your shop with Livewire Brambles.");

                    return Task.CompletedTask;
                }
            }

        }

        [Upgrade]
        public class SpellPrinter : Upgrade
        {
            public SpellPrinter() :
                base(UpgradeSet.ScholomanceAcademy, "Spell Printer", 5, 4, 5, Rarity.Rare, "Spellburst: Add a copy of the spell to your hand.")
            {
                this.effects.Add(new Spellburst());
            }
            private class Spellburst : Effect
            {
                public Spellburst() : base(EffectType.Spellburst, "Spellburst: Add a copy of the spell to your hand.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (caller is Card)
                    {
                        gameHandler.players[curPlayer].hand.AddCard(caller);
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class TightropeChampion : Upgrade
        {
            public TightropeChampion() :
                base(UpgradeSet.ScholomanceAcademy, "Tightrope Champion", 4, 4, 4, Rarity.Rare, "Start of Combat: If your Upgrade's Attack is equal to its Health, gain +2/+2.")
            {
                this.effects.Add(new StartOfCombat());
            }
            private class StartOfCombat : Effect
            {
                public StartOfCombat() : base(EffectType.StartOfCombat, "Start of Combat: If your Upgrade's Attack is equal to its Health, gain +2/+2.", EffectDisplayMode.Public) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    if (gameHandler.players[curPlayer].creatureData.attack == gameHandler.players[curPlayer].creatureData.health)
                    {
                        gameHandler.players[curPlayer].creatureData.attack += 2;
                        gameHandler.players[curPlayer].creatureData.health += 2;
                        if (extraInf is ExtraEffectInfo.StartOfCombatInfo info)
                        {
                            info.output.Add($"{gameHandler.players[curPlayer].name}'s Tighrope Champion triggers and gives it +2/+2, leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
                        }
                    }
                    else
                    {
                        if (extraInf is ExtraEffectInfo.StartOfCombatInfo info)
                        {
                            info.output.Add($"{gameHandler.players[curPlayer].name}'s Tighrope Champion failed to trigger.");
                        }
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class MetallicBroomstick : Upgrade
        {
            public MetallicBroomstick() :
                base(UpgradeSet.ScholomanceAcademy, "Metallic Broomstick", 3, 1, 1, Rarity.Rare, "Rush x2. Aftermath: Give all other Mechs Rush.")
            {
                this.creatureData.staticKeywords[StaticKeyword.Rush] = 2;
                this.effects.Add(new Aftermath());
            }
            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathEnemy, "Aftermath: Give all other Mechs Rush.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    foreach (var player in gameHandler.players)
                    {
                        if (player.Key != curPlayer)
                        {
                            gameHandler.players[player.Key].creatureData.staticKeywords[StaticKeyword.Rush]++;
                            gameHandler.players[player.Key].aftermathMessages.Add(
                                $"{gameHandler.players[curPlayer].name}'s Metallic Broomstick gives you Rush.");
                        }
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class Naptron : Upgrade
        {
            public Naptron() :
                base(UpgradeSet.ScholomanceAcademy, "Naptron", 4, 1, 10, Rarity.Epic, "Taunt x2. Aftermath: Give your Upgrade Rush x2.")
            {
                this.effects.Add(new Aftermath());
            }

            private class Aftermath : Effect
            {
                public Aftermath() : base(EffectType.AftermathMe, "Aftermath: Give your Upgrade Rush x2.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush] += 2;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class EnchantedVendingMachine : Upgrade
        {
            public EnchantedVendingMachine() : 
                base(UpgradeSet.ScholomanceAcademy, "Enchanted Vending Machine", 3, 3, 3, Rarity.Epic, "Spellburst: Refresh your shop.")
            {
                this.effects.Add(new Spellburst());
            }
            private class Spellburst : Effect
            {
                public Spellburst() : base(EffectType.Spellburst, "Spellburst: Refresh your shop.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].shop.Refresh(gameHandler, gameHandler.players[curPlayer].pool, gameHandler.players[curPlayer].maxMana,false);
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class TitaniumBloomer : Upgrade
        {
            public TitaniumBloomer() :
                base(UpgradeSet.ScholomanceAcademy, "Titanium Bloomer", 4, 4, 2, Rarity.Epic, "Battlecry: Add a Lightning Bloom to your hand.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].hand.AddCard(new LightningBloom());
                    return Task.CompletedTask;
                }
            }

            [Token]
            public class LightningBloom : Spell
            {
                public LightningBloom() :
                    base("Lightning Bloom", 0, "Gain 2 Mana this turn only. Overload: (2)")
                {
                    this.effects.Add(new OnPlay());
                }
                private class OnPlay : Effect
                {
                    public OnPlay() : base(EffectType.OnPlay) { }

                    public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        gameHandler.players[curPlayer].curMana += 2;
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload] += 2;
                        return Task.CompletedTask;
                    }
                }
            }
        }

        [Upgrade]
        public class Bibliobot : Upgrade
        {
            public Bibliobot() :
                base(UpgradeSet.ScholomanceAcademy, "Bibliobot", 5, 5, 3, Rarity.Epic, "Bibliobot - Epic - 5/5/3 - Battlecry: Name a letter. This round, after you buy an Upgrade that starts with that letter, gain +2 Attack.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    var playerInteraction = new PlayerInteraction("Name a Letter", string.Empty, "Capitalisation is ignored");

                    string ret = await playerInteraction.SendInteractionAsync(gameHandler, curPlayer, (x, y, z) => x.Count() == 1, ((char)GameHandler.randomGenerator.Next('a', 'z')).ToString(), extraInf.ctx);

                    gameHandler.players[curPlayer].effects.Add(new OnBuyingAMech(ret[0]));
                }

                private class OnBuyingAMech : Effect
                {
                    private char _letter;
                    public OnBuyingAMech() : base(EffectType.OnBuyingAMech, $"After you buy an Upgrade that starts with the letter ' ', gain +2 Attack.", EffectDisplayMode.Private) { }
                    public OnBuyingAMech(char c) :
                        base(EffectType.OnBuyingAMech, $"After you buy an Upgrade that starts with the letter '{c}', gain +2 Attack.", EffectDisplayMode.Private)
                    {
                        this._letter = c;
                    }

                    public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        if (caller.name.Length > 0)
                        {
                            string s = $"{caller.name[0]}";
                            string c = $"{this._letter}";

                            if (s.Equals(c, StringComparison.OrdinalIgnoreCase))
                            {
                                gameHandler.players[curPlayer].creatureData.attack += 2;
                            }
                        }

                        return Task.CompletedTask;
                    }

                    public override Effect Copy()
                    {
                        OnBuyingAMech ret = base.Copy() as OnBuyingAMech;
                        ret._letter = this._letter;
                        return ret;
                    }
                }
            }
        }

        [Upgrade]
        public class ChaosPrism : Upgrade
        {
            public ChaosPrism() :
                base(UpgradeSet.ScholomanceAcademy, "Chaos Prism", 6, 2, 2, Rarity.Legendary, "Taunt x3. Spellburst: Gain \"Spellburst: Gain \'Spellburst: Gain Poisonous.\'\"")
            {
                this.creatureData.staticKeywords[StaticKeyword.Taunt] = 3;
                this.effects.Add(new Spellburst1());
            }
            private class Spellburst1 : Effect
            {
                public Spellburst1() : base(EffectType.Spellburst, "Spellburst: Gain \"Spellburst: Gain \'Spellburst: Gain Poisonous.\'\"", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].effects.Add(new Spellburst2());
                    return Task.CompletedTask;
                }
            }
            private class Spellburst2 : Effect
            {
                public Spellburst2() : base(EffectType.Spellburst, "Spellburst: Gain \'Spellburst: Gain Poisonous.\'", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].effects.Add(new Spellburst3());
                    return Task.CompletedTask;
                }
            }

            private class Spellburst3 : Effect
            {
                public Spellburst3() : base(EffectType.Spellburst, "Spellburst: Gain Poisonous.", EffectDisplayMode.Private) { }

                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Poisonous] = 1;
                    return Task.CompletedTask;
                }
            }
        }

        [Upgrade]
        public class LordBarox : Upgrade
        {
            public LordBarox() :
                base(UpgradeSet.ScholomanceAcademy, "Lord Barox", 3, 3, 2, Rarity.Legendary, "Battlecry: Name ANY other Mech. Aftermath: If it won last round, gain 5 Mana this turn only.")
            {
                this.effects.Add(new Battlecry());
            }
            private class Battlecry : Effect
            {
                public Battlecry() : base(EffectType.Battlecry) { }

                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    List<ulong> userIds = new List<ulong>();
                    foreach (var player in gameHandler.players)
                    {
                        if (player.Key != curPlayer) userIds.Add(player.Key);
                    }

                    if (userIds.Count() == 0) return;

                    string desc = string.Empty;

                    for (int i=0; i<userIds.Count(); i++)
                    {
                        if (i != 0) desc += '\n';
                        desc += $"{i+1}) {gameHandler.players[userIds[i]].name}";
                    }

                    var prompt = new PlayerInteraction("Name a Player other than yourself", desc, "Type the corresponding index");

                    string defaultAns = (GameHandler.randomGenerator.Next(userIds.Count()) + 1).ToString();

                    string ret = (await prompt.SendInteractionAsync(gameHandler, curPlayer, (x, y, z) => GeneralFunctions.Within(x, 1, userIds.Count), defaultAns, extraInf.ctx)).ToLower();

                    ulong chosen = userIds[int.Parse(ret) - 1];

                    gameHandler.players[curPlayer].effects.Add(new Aftermath(chosen, gameHandler.players[chosen].name));
                }
                private class Aftermath : Effect
                {
                    private ulong chosen;

                    public Aftermath() : base(EffectType.Null) { this.chosen = 0; }
                    public Aftermath(ulong chosen, string name) : 
                        base(EffectType.AftermathMe, $"You have bet on {name}! If they win their next fight, you'll gain 5 Mana next turn.", EffectDisplayMode.Private) 
                    {
                        this.chosen = chosen;
                    }

                    public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                    {
                        if (this.chosen == 0) return Task.CompletedTask;

                        if (gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count()-2][this.chosen] == FightResult.WIN)
                        {
                            gameHandler.players[curPlayer].curMana += 5;
                            gameHandler.players[curPlayer].aftermathMessages.Add(
                                $"The bet on your Lord Barox for {gameHandler.players[this.chosen].name} was correct! You gain 5 Mana this turn only.");
                        }
                        else
                        {
                            gameHandler.players[curPlayer].aftermathMessages.Add(
                                $"The bet on your Lord Barox for {gameHandler.players[this.chosen].name} was incorrect.");
                        }

                        return Task.CompletedTask;
                    }

                    public override Effect Copy()
                    {
                        Aftermath ret = (Aftermath)base.Copy();
                        ret.chosen = this.chosen;                         
                        return ret;
                    }
                }
            }
        }
    }
}
