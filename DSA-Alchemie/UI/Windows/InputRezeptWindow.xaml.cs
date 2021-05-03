using Alchemie.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace Alchemie.UI.Windows
{
    /// <summary>
    /// Interaktionslogik für AddRezeptWindow.xaml
    /// </summary>
    public partial class InputRezeptWindow : Window
    {
        public Models.Rezept NewRezept { private set; get; }
        private Wirkung wirkung;

        public InputRezeptWindow()
        {
            InitializeComponent();
        }

        public InputRezeptWindow(Models.Rezept rezept)
        {
            NewRezept = new Models.Rezept(rezept);
            Set();
            InitializeComponent();
        }

        private void Set()
        {
            nameIN.Text = NewRezept.Name;
            gruppeIN.Text = NewRezept.Gruppe;
            laborIN.SelectedIndex = (int)NewRezept.Labor.ID;
            propeIN_B.IntValue = NewRezept.Probe.BrauenMod; propeIN_A.IntValue = NewRezept.Probe.AnalyseMod;
            verbrIN.Text = NewRezept.Verbreitung ?? String.Empty;
            haltbIN.Text = NewRezept.Haltbarkeit ?? String.Empty;
            merkmIN.Text = NewRezept.Merkmale ?? String.Empty;
            zutatenIN.Text = NewRezept.Rezeptur ?? String.Empty;
            seiteIN.IntValue = NewRezept.Seite;
            beschIN_T.Text = NewRezept.Beschaffung.Preis;
            beschIN_V.IntValue = Int32.Parse(NewRezept.Beschaffung.Verbreitung, CultureInfo.CurrentCulture);
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
            NewRezept = new Models.Rezept(nameIN.Text, gruppeIN.Text, laborIN.SelectedIndex.ToString(CultureInfo.CurrentCulture), (propeIN_B.IntValue, propeIN_A.IntValue))
            {
                Verbreitung = verbrIN.Text.Length != 0 ? verbrIN.Text : null,
                Haltbarkeit = haltbIN.Text.Length != 0 ? haltbIN.Text : null,
                Merkmale = merkmIN.Text.Length != 0 ? merkmIN.Text : null,
                Rezeptur = zutatenIN.Text.Length != 0 ? zutatenIN.Text : null,
                Seite = seiteIN.IntValue,
                Beschaffung = new Beschaffung(beschIN_T.Text, beschIN_V.IntValue.ToString(CultureInfo.CurrentCulture)),
                Preis = preisIN.Text.Length != 0 ? preisIN.Text : null,
                Wirkung = wirkung
            };
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