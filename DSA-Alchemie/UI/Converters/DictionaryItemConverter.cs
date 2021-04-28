using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Alchemie.UI.Converters
{
    public class DictionaryItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type type, object paramater, CultureInfo culture)
        {
            if (values != null && values.Length >= 2)
            {
                if (values[0] is IDictionary && !(values is null))
                {
                    var myDict = values[0] as IDictionary;
                    var myKey = values[1];
                    return myDict[myKey].ToString();
                }
            }
            return Binding.DoNothing;
        }
        public object[] ConvertBack(object value, Type[] type, object paramater, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
