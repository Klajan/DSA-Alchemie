using System;
using System.Windows.Data;
using System.Globalization;

namespace Alchemie.UI.Converters
{
    public class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
    }
}
