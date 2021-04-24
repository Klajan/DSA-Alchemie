using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace Alchemie
{
    public static class Helper
    {
        public static int CalcLaborMod(int requLab, int currentLab)
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
    public class AstralAufladenConverter : IValueConverter
    {
        public object Convert (object value, Type type, object paramater, CultureInfo culture)
        {
            if (!(value is int)) { return ""; }
            if ((int)value <= 0) { return "0"; }
            return (Math.Pow(2, (int)value-1)).ToString(CultureInfo.CurrentCulture);
        }
        public object ConvertBack (object value, Type type, object paramater, CultureInfo culture)
        {
            if (!(value is string)) { return 0; }
            int z;
            if (Int32.TryParse((string)value, out z)) { return Math.Log(z, 2); }
            return 0;
        }
    }
}
