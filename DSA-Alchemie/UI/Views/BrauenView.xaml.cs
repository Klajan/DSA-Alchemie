using Alchemie.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Alchemie.Models.Types;

namespace Alchemie.UI.Views
{
    /// <summary>
    /// Interaktionslogik für BrauenView.xaml
    /// </summary>
    public partial class BrauenView : UserControl
    {
        public BrauenView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //int mod = (int)((Tuple<Subsitution, string>)SubstitutionBox.SelectedItem).Item1;
            int mod = (int)BrauenViewModel.Subsitution;
            BrauenViewModel.Trank.Brauen(mod, (BrauenViewModel.Zurückhalten, BrauenViewModel.AstralAufladen, BrauenViewModel.MiscMod));
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "instantiated in xaml")]
    internal class ZurückhaltenCalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (values.Length >= 3 && values[0] is int mod && values[1] is LaborID lab1 && values[2] is LaborID lab2)
            {
                return (int)Math.Max(0, Math.Ceiling(System.Convert.ToDouble(mod) * 1.5) - Trank.CalculateLaborMod(lab1, lab2));
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "instantiated in xaml")]
    internal class AstralAufladenConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (!(values[0] is int)) { return 0; }
            if ((int)values[0] <= 0) { return 0; }
            return Math.Max(0, System.Convert.ToInt32(Math.Min(Int32.MaxValue, Math.Round(Math.Pow(2, (int)values[0] - 1) / (values.Length > 1 ? (bool)values[1] ? 2.0 : 1.0 : 1.0), MidpointRounding.AwayFromZero))));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "instantiated in xaml")]
    internal class TotalModCalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            int mod = 0;
            if (values.Length > 3 && values[0] is LaborID lab1 && values[1] is LaborID lab2 && values[2] is bool chym)
            {
                mod = Trank.CalculateLaborMod(lab1, lab2) + (chym ? -1 : 0);
                for (int i = 3; i < values.Length; i++)
                {
                    mod += System.Convert.ToInt32(values[i], culture);
                }
            }
            return mod;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}