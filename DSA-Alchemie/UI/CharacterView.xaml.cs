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
using System.Globalization;
using Alchemie.common;
using Alchemie.UI.ViewModels;

namespace Alchemie.UI
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


        public static readonly Tuple<LabQual, string>[] LabQualityList = new Tuple<LabQual, string>[]
        {
            Tuple.Create(LabQual.Fehlend, "(+3) Fehlende/beschädigte Gerätschaften"),
            Tuple.Create(LabQual.Normal, "(+0) Normales Labor"),
            Tuple.Create(LabQual.Gut, "(-3) Hochwertiges Labor"),
            Tuple.Create(LabQual.SehrGut, "(-7) Außergewöhnlich hochwertiges Labor")
        };

        public static readonly Tuple<LabLvl, string>[] LabLevelList = new Tuple<LabLvl, string>[]
        {
            Tuple.Create(LabLvl.ArchaischesLabor, "Archaisches Labor"),
            Tuple.Create(LabLvl.Hexenküche, "Hexenküche"),
            Tuple.Create(LabLvl.Alchemielabor, "Alchimistenlabor")
        };
    }

    internal class LabQualityIndexConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture)
        {
            switch ((LabQual)value)
            {
                case LabQual.Fehlend:
                    return 0;
                case LabQual.Normal:
                    return 1;
                case LabQual.Gut:
                    return 2;
                case LabQual.SehrGut:
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
                    return LabQual.Fehlend;
                case 1:
                    return LabQual.Normal;
                case 2:
                    return LabQual.Gut;
                case 3:
                    return LabQual.SehrGut;
                default:
                    return LabQual.Normal;
            }
        }
    }

    internal class LabLevelIndexConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture)
        {
            return (int)value;
        }
        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture)
        {
            return (LabLvl)value;
        }
    }
}
