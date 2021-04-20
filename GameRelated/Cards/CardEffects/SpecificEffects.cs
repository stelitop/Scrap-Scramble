using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects
{
    public class SpecificEffects
    {
        public bool hideUpgradesInLog;
        public bool invertAttackPriority;
        public bool ignoreSpikes, ignoreShields;
        public int multiplierStartOfCombat;
        public bool invertLossPenalty;

        public SpecificEffects()
        {
            hideUpgradesInLog = false;
            invertAttackPriority = false;
            ignoreShields = false;
            ignoreSpikes = false;
            multiplierStartOfCombat = 1;
            invertLossPenalty = false;
        }
    }
}
