using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects.Effect;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects
{
    public class ExtraEffectInfo
    {
        public CommandContext ctx;
        public EffectType calledEffect = EffectType.Null;

        public ExtraEffectInfo(CommandContext ctx)
        {
            this.ctx = ctx;
        }

        public class StartOfCombatInfo : ExtraEffectInfo
        {
            public List<string> output;
            public ulong firstPlayer;

            public StartOfCombatInfo(CommandContext ctx, ulong player) : base(ctx) 
            {
                this.output = new List<string>();
                this.firstPlayer = player;
            }                        
        }

        public class DamageInfo : ExtraEffectInfo
        {
            public int dmg;
            public string output;

            public DamageInfo(CommandContext ctx, int dmg, string output = "") : base(ctx)
            {
                this.dmg = dmg;
                this.output = output;
            }
        }

        public class AfterAttackInfo : ExtraEffectInfo
        {
            public List<string> output;
            public int dmg;

            public AfterAttackInfo(CommandContext ctx, int damage) : base(ctx)
            {
                this.output = new List<string>();
                this.dmg = damage;
            }

        }
    }

    //public class AttackEffectExtraInformation : ExtraEffectInformation
    //{
    //    public AttackEffectExtraInformation(CommandContext ctx)
    //    {

    //    }
    //}
}
