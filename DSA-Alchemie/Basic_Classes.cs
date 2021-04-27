using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using Alchemie.Models;

namespace Alchemie
{
    public static class Helper
    {
        public static int CalcLaborMod(LaborID requLab, LaborID currentLab)
        {
            var diff = requLab - currentLab;
            switch (requLab - currentLab)
            {
                case -2:
                    return -3;
                case -1:
                    return 0;
                case 0:
                    return 0;
                case +1:
                    return +7;
                default:
                    return +99;
            }
        }
        public static int CalcAstralAufladen(int asp, bool ChymischeHochzeit)
        {
            var mod = (int)(Math.Log(asp, 2) + 1);
            return (ChymischeHochzeit) ? mod * 2 : mod;
        }
        public static int GetLabQuality() { return 0; }
    }
}
