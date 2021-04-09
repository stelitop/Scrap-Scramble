using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated
{
    public class FightOutputCollector
    {
        public List<string> playerInfoOutput1;
        public List<string> playerInfoOutput2;

        public List<string> preCombatOutput;
        public List<string> combatOutput;

        public ulong goingFirst;

        public FightOutputCollector()
        {
            this.playerInfoOutput1 = new List<string>();
            this.playerInfoOutput2 = new List<string>();

            this.preCombatOutput = new List<string>();
            this.combatOutput = new List<string>();

            this.goingFirst = 0;
        }
    }
}
