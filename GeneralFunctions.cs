using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version
{
    public static class GeneralFunctions
    {
        public static void Swap<T>(ref T x, ref T y)
        {
            T t = x;
            x = y;
            y = t;
        }

        public static bool Within(string s, int a, int b)
        {
            if (int.TryParse(s, out int parsed))
            {
                return (a <= parsed && parsed <= b);
            }
            else return false;
        }
    }
}
