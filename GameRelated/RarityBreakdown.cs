using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated
{
    [Serializable]
    public struct RarityBreakdown
    {
        public int common, rare, epic, legendary;

        public RarityBreakdown(int c, int r, int e, int l)
        {
            this.common = c;
            this.rare = r;
            this.epic = e;
            this.legendary = l;
        }
    };
}
