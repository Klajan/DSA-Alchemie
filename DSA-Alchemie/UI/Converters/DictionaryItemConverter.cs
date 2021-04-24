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
            if(values != null && values.Length >= 2)
            {
                var myDict = values[0] as IDictionary;
                var myKey = values[1];
                if (myDict != null && myKey != null)
                {
                    //the automatic conversion from Uri to string doesn't work
                    //return myDict[myKey];
                    return myDict[myKey].ToString();
                }
            }
            return Binding.DoNothing;
        }
        public object[] ConvertBack(object value, Type[] type, object paramater, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
