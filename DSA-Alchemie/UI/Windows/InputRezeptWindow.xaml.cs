using Alchemie.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Alchemie.UI.Windows
{
    /// <summary>
    /// Interaktionslogik für AddRezeptWindow.xaml
    /// </summary>
    public partial class InputRezeptWindow : Window
    {
        public common.Rezept NewRezept { private set; get; }
        private Wirkung wirkung;
        public InputRezeptWindow()
        {
            InitializeComponent();
        }
        public InputRezeptWindow(common.Rezept rezept)
        {
            NewRezept = new common.Rezept(rezept);
            Set();
            InitializeComponent();
        }

        private void Set()
        {
            nameIN.Text = NewRezept.Name;
            gruppeIN.Text = NewRezept.Gruppe;
            laborIN.SelectedIndex = NewRezept.Labor.ID;
            propeIN_B.Value = NewRezept.Probe.BrauenMod; propeIN_A.Value = NewRezept.Probe.AnalyseMod;
            verbrIN.Text = NewRezept.Verbreitung ?? String.Empty;
            haltbIN.Text = NewRezept.Haltbarkeit ?? String.Empty;
            merkmIN.Text = NewRezept.Merkmale != null ? NewRezept.Merkmale : string.Empty;
            zutatenIN.Text = NewRezept.Rezeptur ?? String.Empty;
            seiteIN.Value = NewRezept.Seite;
            beschIN_T.Text = NewRezept.Beschaffung.Preis;
            beschIN_V.Value = Int32.Parse(NewRezept.Beschaffung.Verbreitung, CultureInfo.CurrentCulture);
            preisIN.Text = NewRezept.Preis ?? String.Empty;
            wirkung = NewRezept.Wirkung;
        }
        private bool Get()
        {
            if (nameIN.Text.Length == 0 || gruppeIN.Text.Length == 0 || laborIN.SelectedIndex == -1)
            {
                //throw some error and exit
                NewRezept = null;
                return false;
            }
            NewRezept = new common.Rezept(nameIN.Text, gruppeIN.Text, laborIN.SelectedIndex.ToString(CultureInfo.CurrentCulture), (propeIN_B.Value, propeIN_A.Value));
            NewRezept.Verbreitung = verbrIN.Text.Length != 0 ? verbrIN.Text : null;
            NewRezept.Haltbarkeit = haltbIN.Text.Length != 0 ? haltbIN.Text : null;
            NewRezept.Merkmale = merkmIN.Text.Length != 0 ? merkmIN.Text : null;
            NewRezept.Rezeptur = zutatenIN.Text.Length != 0 ? zutatenIN.Text : null;
            NewRezept.Seite = seiteIN.Value;
            NewRezept.Beschaffung = new Beschaffung(beschIN_T.Text, beschIN_V.Value.ToString(CultureInfo.CurrentCulture));
            NewRezept.Preis = preisIN.Text.Length != 0 ? preisIN.Text : null;
            NewRezept.Wirkung = wirkung;
            return true;
        }

        public static RoutedCommand ExitCommand { private set; get; } = new RoutedCommand();
        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Get();
            this.Close();
        }
        public static RoutedCommand OpenWirkungInputCommand { private set; get; } = new RoutedCommand();
        private void WirkungInputCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void WirkungInputCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var window = new InputWirkungWindow();
            window.ShowDialog();
            wirkung = window.Wirkung;
        }
    }
}
