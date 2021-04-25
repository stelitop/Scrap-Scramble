using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects
{
    public abstract class Effect
    {
        public enum EffectDisplayMode
        {
            Hidden = 0,
            Private = 1,
            Public = 2,
        }
        public enum EffectType
        {
            Null,
            Battlecry,
            AftermathMe,
            AftermathEnemy,
            StartOfCombat,
            OnPlay,
            OnSpellCast,
            Spellburst, 
            OnBuyingAMech,
            Combo,
            AfterThisAttacks,
            AfterThisTakesDamage,
            BeforeTakingDamage,
            AfterTheEnemyAttacks,
            EndOfTurnInHand,
            OnBeingFrozen,
            OnFriendlyStartOfCombatTrigger,
            OnEnemyStartOfCombatTrigger,
        }

        public string effectText;
        public EffectDisplayMode displayMode;

        protected bool _toBeRemoved = false;

        private List<EffectType> _type;
        public List<EffectType> Type { get { return _type; } private set { _type = value; } }

        public Effect()
        {
            this.effectText = string.Empty;
            this.displayMode = EffectDisplayMode.Hidden;
            this.Type = new List<EffectType>();
        }
        public Effect(EffectType type, string effectText = "", EffectDisplayMode displayMode = EffectDisplayMode.Hidden)
        {
            this._type = new List<EffectType> { type };
            this.effectText = effectText;
            this.displayMode = displayMode;
        }
        public Effect(EffectType[] types, string effectText = "", EffectDisplayMode displayMode = EffectDisplayMode.Hidden)
        {
            this._type = types.ToList();
            this.effectText = effectText;
            this.displayMode = displayMode;
        }

        public virtual Effect Copy()
        {
            Effect ret = (Effect)Activator.CreateInstance(this.GetType());
            ret.effectText = this.effectText;
            ret.displayMode = this.displayMode;
            ret._type = this._type;
            return ret;
        }
        
        public abstract Task Call(Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf);

        public static async Task CallEffects(List<Effect> effects, EffectType type, Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf, bool removeAfterCall = false)
        {
            List<Effect> toBeCast = new List<Effect>();

            if (extraInf != null) extraInf.calledEffect = type;

            for (int i=0; i<effects.Count(); i++)
            {
                if (effects[i].Type.Contains(type))
                {
                    toBeCast.Add(effects[i]);
                }
            }

            if (removeAfterCall) effects.RemoveAll(x => x.Type.Contains(type));

            foreach (var effect in toBeCast)
            {
                await effect.Call(caller, gameHandler, curPlayer, enemy, extraInf);

                await CallSubEffects(effects, type, caller, gameHandler, curPlayer, enemy, extraInf);
            }

            if (!removeAfterCall)
            for (int i = 0; i < effects.Count(); i++)
            {
                if (effects[i].ToBeRemoved())
                {
                    effects.RemoveAt(i);
                    i--;
                }
            }
        }

        private static async Task CallSubEffects(List<Effect> effects, EffectType type, Card caller, GameHandler gameHandler, ulong curPlayer, ulong enemy, ExtraEffectInfo extraInf)
        {
            switch (type)
            {
                case EffectType.StartOfCombat:

                    await CallEffects(effects, EffectType.OnFriendlyStartOfCombatTrigger, caller, gameHandler, curPlayer, enemy, extraInf);
                    await CallEffects(effects, EffectType.OnEnemyStartOfCombatTrigger, caller, gameHandler, enemy, curPlayer, extraInf);

                    break;
                default:
                    break;
            }
        }

        public bool ToBeRemoved()
        {
            return this._toBeRemoved;
        }
    }
}
