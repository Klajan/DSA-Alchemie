using Alchemie.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Alchemie.UI.Views
{
    /// <summary>
    /// Interaktionslogik für BrauenView.xaml
    /// </summary>
    public partial class BrauenView : UserControl
    {
        public static readonly Tuple<Subsitution, string>[] SubstitutionTemplate = new Tuple<Subsitution, string>[]
        {
            Tuple.Create(Subsitution.Optimierend, "(-3) Optimierende Substitution" ),
            Tuple.Create(Subsitution.Gleichwertig, "(+0) Gleichwertige Substitution"),
            Tuple.Create(Subsitution.Sinnvoll, "(+3) Sinnvolle Substitution"),
            Tuple.Create(Subsitution.Möglich, "(+6) Mögliche Substitution"),
            Tuple.Create(Subsitution.Unsinnig, "Unsinnige Substitution")
        };

        public static readonly char[] QualityTemplate = { 'M', 'A', 'B', 'C', 'D', 'E', 'F' };

        public BrauenView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //int mod = (int)((Tuple<Subsitution, string>)SubstitutionBox.SelectedItem).Item1;
            int mod = (int)BrauenViewModel.Subsitution;
            char q = BrauenViewModel.Trank.Brauen(mod, (BrauenViewModel.Zurückhalten, BrauenViewModel.AstralAufladen, BrauenViewModel.MiscMod));
        }
    }

    internal class ZurückhaltenCalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object paramater, CultureInfo culture)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            int mod = 0;
            LaborID lab1 = 0;
            LaborID lab2 = 0;
            if (values.Length >= 3 && values[0] is int && values[1] is LaborID && values[2] is LaborID)
            {
                mod = (int)values[0];
                lab1 = (LaborID)values[1];
                lab2 = (LaborID)values[2];
            }
            return (int)Math.Max(0, Math.Ceiling(System.Convert.ToDouble(mod) * 1.5) - Helper.CalcLaborMod(lab1, lab2));
        }

        public object[] ConvertBack(object value, Type[] type, object paramater, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class AstralAufladenConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object paramater, CultureInfo culture)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (!(values[0] is int)) { return 0; }
            if ((int)values[0] <= 0) { return 0; }
            return Math.Max(0, System.Convert.ToInt32(Math.Min(Int32.MaxValue, Math.Round(Math.Pow(2, (int)values[0] - 1) / (values.Length > 1 ? (bool)values[1] ? 2.0 : 1.0 : 1.0), MidpointRounding.AwayFromZero))));
        }

        public object[] ConvertBack(object value, Type[] type, object paramater, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class TotalModCalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            int mod = 0;
            if (values.Length > 3 && values[0] is LaborID && values[1] is LaborID && values[2] is bool)
            {
                mod = Trank.CalculateLaborMod((LaborID)values[0], (LaborID)values[1]) + ((bool)values[2] ? -1 : 0);
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