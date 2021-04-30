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
            Tuple.Create(LaborID.ArchaischesLabor, "Archaisches Labor"),
            Tuple.Create(LaborID.Hexenküche, "Hexenküche"),
            Tuple.Create(LaborID.Alchemielabor, "Alchimistenlabor")
        };
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "instantiated in xaml")]
    internal class LabQualityIndexConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture)
        {
            switch ((LaborQL)value)
            {
                case LaborQL.Fehlend:
                    return 0;
                case LaborQL.Normal:
                    return 1;
                case LaborQL.Gut:
                    return 2;
                case LaborQL.SehrGut:
                    return 3;
                default:
                    return 1;
            }
        }
        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return LaborQL.Fehlend;
                case 1:
                    return LaborQL.Normal;
                case 2:
                    return LaborQL.Gut;
                case 3:
                    return LaborQL.SehrGut;
                default:
                    return LaborQL.Normal;
            }
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
