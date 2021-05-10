using Alchemie.Models;
using Alchemie.UI.ViewModels;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Alchemie.UI.Views
{
    /// <summary>
    /// Interaktionslogik für CharacterView.xaml
    /// </summary>
    public partial class CharacterView : UserControl
    {
        public CharacterViewModel CharacterModel { get { return CharacterViewModel; } }

        public CharacterView()
        {
            InitializeComponent();
        }

        public static readonly Tuple<LaborQL, string>[] LabQualityList = new Tuple<LaborQL, string>[]
        {
            Tuple.Create(LaborQL.Fehlend, "(+3) Fehlende/beschädigte Gerätschaften"),
            Tuple.Create(LaborQL.Normal, "(+0) Normales Labor"),
            Tuple.Create(LaborQL.Gut, "(-3) Hochwertiges Labor"),
            Tuple.Create(LaborQL.SehrGut, "(-7) Außergewöhnlich hochwertiges Labor")
        };

        public static readonly Tuple<LaborID, string>[] LabLevelList = new Tuple<LaborID, string>[]
        {
            Tuple.Create(LaborID.ArchaischesLabor, "archaisches Labor"),
            Tuple.Create(LaborID.Hexenküche, "Hexenküche"),
            Tuple.Create(LaborID.Alchemielabor, "Alchimistenlabor")
        };
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "instantiated in xaml")]
    internal class LabQualityIndexConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture)
        {
            return (LaborQL)value switch
            {
                LaborQL.Fehlend => 0,
                LaborQL.Normal => 1,
                LaborQL.Gut => 2,
                LaborQL.SehrGut => 3,
                _ => 1,
            };
        }

        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture)
        {
            return (int)value switch
            {
                0 => LaborQL.Fehlend,
                1 => LaborQL.Normal,
                2 => LaborQL.Gut,
                3 => LaborQL.SehrGut,
                _ => LaborQL.Normal,
            };
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "instantiated in xaml")]
    internal class LabLevelIndexConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture)
        {
            return (LaborID)value;
        }
    }
}