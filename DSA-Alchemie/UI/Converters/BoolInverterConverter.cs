using System;
using System.Globalization;
using System.Windows.Data;

namespace Alchemie.UI.Converters
{
    public class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
    }
}
