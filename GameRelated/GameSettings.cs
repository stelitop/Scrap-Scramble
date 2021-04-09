using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated
{    
    public class GameSettings
    {
        public int startingLives;
        public RarityBreakdown rarityBreakdown;
        public int maxManaCap;
        public int startingMana;
        public int setAmount;

        public GameSettings()
        {
            this.startingLives = 4;
            this.setAmount = 8;
            this.startingMana = 10;
            this.maxManaCap = 30;
            this.rarityBreakdown = new RarityBreakdown(4, 3, 2, 1);
        }
    }
}
