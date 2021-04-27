using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Alchemie.Models;

namespace Alchemie
{
    public class RuckhaltenCalculator : IMultiValueConverter
    {
        public object Convert(object[] value, Type type, object paramater, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            int mod = 0;
            LaborID lab1 = 0;
            LaborID lab2 = 0;
            if (value.Length >= 3 && value[0] is int && value[1] is LaborID && value[2] is LaborID)
            {
                mod = System.Convert.ToInt32(value[0], CultureInfo.CurrentCulture);
                lab1 = (LaborID)value[1];
                lab2 = (LaborID)value[2];
            }
            //return 3
            return (int)Math.Max(0, Math.Ceiling(System.Convert.ToDouble(mod) * 1.5) - Helper.CalcLaborMod(lab1, lab2));
        }
        public object[] ConvertBack(object value, Type[] type, object paramater, CultureInfo culture) { throw new NotImplementedException(); }
    }
    public class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
    }
}