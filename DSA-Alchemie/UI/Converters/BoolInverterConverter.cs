using System;
using System.Globalization;

namespace Alchemie.UI.Converters
{
    class BoolInverterConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture) { return !(bool)value; }
    }
}
