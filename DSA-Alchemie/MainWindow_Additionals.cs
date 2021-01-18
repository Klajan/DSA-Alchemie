using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Alchemie
{
    class RuckhaltenCalculator : IMultiValueConverter
    {
        public object Convert(object[] value, Type type, object paramater, CultureInfo culture)
        {
            int mod = 0;
            int lab1 = 0;
            int lab2 = 0;
            if (value.Length >= 3 && value[0] is int && value[1] is int && value[2] is int)
            {
                mod = System.Convert.ToInt32(value[0]);
                lab1 = System.Convert.ToInt32(value[1]);
                lab2 = System.Convert.ToInt32(value[2]);
            }
            //return 3
            return (int)Math.Max(0, Math.Ceiling(System.Convert.ToDouble(mod) * 1.5) - Helper.CalcLaborMod(lab1, lab2));
        }
        public object[] ConvertBack(object value, Type[] type, object paramater, CultureInfo culture) { throw new NotImplementedException(); }
    }
    class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
    }
}