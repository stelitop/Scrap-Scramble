using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades
{
    public class SpareParts
    {
        [SparePartAttribute]
        public class SPWhirlingBlades : Spell
        {
            public SPWhirlingBlades() :
                base("Whirling Blades", 1, "Give your Mech +2 Attack and +4 Spikes.", SpellRarity.Spare_Part)
            {
                this.effects.Add(new OnPlay());
            }

            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) {}
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.attack += 2;
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
                    return Task.CompletedTask;
                }
            }            
        }

        [SparePartAttribute]
        public class SPArmorPlating : Spell
        {
            public SPArmorPlating() :
                base("Armor Plating", 1, "Give your Mech +2 Health and +4 Shields.", SpellRarity.Spare_Part)
            {
                this.effects.Add(new OnPlay());
            }

            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.health += 2;
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
                    return Task.CompletedTask;
                }
            }            
        }

        [SparePartAttribute]
        public class SPReversingSwitch : Spell
        {
            public SPReversingSwitch() :
                   base("Reversing Switch", 1, "Swap your Mech's Attack and Health", SpellRarity.Spare_Part)
            {
                this.effects.Add(new OnPlay());
            }

            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    int mid = gameHandler.players[curPlayer].creatureData.attack;
                    gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
                    gameHandler.players[curPlayer].creatureData.health = mid;
                    return Task.CompletedTask;
                }
            }            
        }

        [SparePartAttribute]
        public class SPTimeAccelerator : Spell
        {
            public SPTimeAccelerator() :
                base("Time Accelerator", 1, "Give your Mech Rush.", SpellRarity.Spare_Part)
            {
                this.effects.Add(new OnPlay());
            }

            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush]++;
                    return Task.CompletedTask;
                }
            }            
        }

        [SparePartAttribute]
        public class SPRustyHorn : Spell
        {
            public SPRustyHorn() :
                base("Rusty Horn", 1, "Give your Mech +3/+3 and Taunt.", SpellRarity.Spare_Part)
            {
                this.effects.Add(new OnPlay());
            }

            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Taunt]++;
                    gameHandler.players[curPlayer].creatureData.attack += 3;
                    gameHandler.players[curPlayer].creatureData.health += 3;
                    return Task.CompletedTask;
                }
            }          
        }

        [SparePartAttribute]
        public class SPManaCapsule : Spell
        {
            public SPManaCapsule() :
                base("Mana Capsule", 1, "Gain 2 Mana this turn only.", SpellRarity.Spare_Part)
            {
                this.effects.Add(new OnPlay());
            }

            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }
                public override Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    gameHandler.players[curPlayer].curMana += 2;
                    return Task.CompletedTask;
                }
            }            
        }

        [SparePartAttribute]
        public class SPEmergencyCoolant : Spell
        {
            public SPEmergencyCoolant() :
                base("Emergency Coolant", 1, "Freeze an Upgrade. Give it +4/+4.", SpellRarity.Spare_Part)
            {
                this.effects.Add(new OnPlay());
            }

            private class OnPlay : Effect
            {
                public OnPlay() : base(EffectType.OnPlay) { }
                public override async Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
                {
                    Upgrade chosen = await PlayerInteraction.FreezeUpgradeInShopAsync(gameHandler, curPlayer, enemy, extraInf.ctx, 1);

                    chosen.creatureData.attack += 4;
                    chosen.creatureData.health += 4;
                }
            }            
        }
    }
}
