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
            return input += 1;
        }
        private int numericDecrease1(int input)
        {
            return input -= 1;
        }
    }
    class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
    }
}