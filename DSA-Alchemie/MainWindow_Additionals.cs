using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Threading;
using System.Globalization;

namespace DSA_Alchemie
{
    public partial class MainWindow : Window
    {
        private int numericIncrease1(int input)
        {
            return input + 1;
        }
        private int numericDecrease1(int input)
        {
            return input - 1;
        }
    }
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