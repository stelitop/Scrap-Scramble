using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards
{
    public abstract class Card
    {
        public string name;
        public string cardText;

        private int cost;
        public int Cost { get { return cost; } set { cost = Math.Max(0, value); } }

        public List<Effect> effects = new List<Effect>();

        public abstract string GetInfo(GameHandler gameHandler, ulong player);
        public abstract Card DeepCopy();
        public abstract Task<bool> PlayCard(int handPos, GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx);

        public virtual bool CanBePlayed(int handPos, GameHandler gameHandler, ulong curPlayer, ulong enemy)
        {
            if (handPos >= gameHandler.players[curPlayer].hand.LastIndex) return false;
            if (this.name == BlankUpgrade.name) return false;
            //if (this.inLimbo) return false;
            if (this.Cost > gameHandler.players[curPlayer].curMana) return false;

            return true;
        }

    }
}
