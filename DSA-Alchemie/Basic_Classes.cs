using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace DSA_Alchemie
{
    public class Character : NotifyPropertyChanged
    {
        int MU_; int KL_; int FF_; int IN_; int labor_; int laborQuality_;
        public int MU { get { return MU_; } set { MU_ = Math.Max(value, 1); } }
        public int KL { get { return KL_; } set { KL_ = Math.Max(value, 1); } }
        public int FF { get { return FF_; } set { FF_ = Math.Max(value, 1); } }
        public int IN { get { return IN_; } set { IN_ = Math.Max(value, 1); } }
        public int alch { set; get; }
        public int koch { set; get; }
        public int labor { get { return labor_; } set { labor_ = Math.Max(Math.Min(value, 2), 0); } }
        public int laborQuality { get { return laborQuality_; } set { laborQuality_ = Math.Max(Math.Min(value, +3), -7); } }
        public (bool AllegorischeAnalyse, bool ChymischeHochzeit, bool MandriconsBindung) schalenzauber { set; get; }
        public Character() { MU = 10; KL = 10; IN = 10; FF = 10; alch = 0; koch = 0; schalenzauber = (false, false, false); }
    }
    public class Helper
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
    class AstralAufladenConverter : IValueConverter
    {
        public object Convert (object value, Type type, object paramater, CultureInfo culture)
        {
            if (!(value is int)) { return ""; }
            if ((int)value <= 0) { return "0"; }
            return (Math.Pow(2, (int)value-1)).ToString();
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
