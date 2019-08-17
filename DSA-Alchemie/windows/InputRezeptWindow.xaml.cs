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
using System.Windows.Shapes;

namespace DSA_Alchemie.window
{
    /// <summary>
    /// Interaktionslogik für AddRezeptWindow.xaml
    /// </summary>
    public partial class InputRezeptWindow : Window
    {
        public common.Rezept NewRezept { private set; get; }
        private string[] wirkung = null;
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
        protected int IncreaseFunc(int input)
        {
            return input + 1;
        }
        protected int DecreaseFunc(int input)
        {
            return input - 1;
        }

        private void Set()
        {
            nameIN.Text = NewRezept.Name;
            gruppeIN.Text = NewRezept.Gruppe;
            laborIN.SelectedIndex = NewRezept.Labor.Item1;
            propeIN_B.Value = NewRezept.Mods.Item1; propeIN_A.Value = NewRezept.Mods.Item2;
            verbrIN.Text = NewRezept.Verbreitung ?? String.Empty;
            haltbIN.Text = NewRezept.Haltbarkeit ?? String.Empty;
            merkmIN.Text = NewRezept.Merkmale != null ? NewRezept.Haltbarkeit : string.Empty;
            zutatenIN.Text = NewRezept.Zutaten ?? String.Empty;
            seiteIN.Value = NewRezept.Seite;
            beschIN_T.Text = NewRezept.Beschaffung != null ? NewRezept.Beschaffung.Item1 : String.Empty;
            beschIN_V.Value = NewRezept.Beschaffung != null ? NewRezept.Beschaffung.Item2 : 0;
            preisIN.Text = NewRezept.Preis ?? String.Empty;
            wirkung = NewRezept.Wirkung.Count == 0 ? NewRezept.Wirkung.Values.ToArray() : null;
        }
        private bool Get()
        {
            if (nameIN.Text.Length == 0 || gruppeIN.Text.Length == 0 || laborIN.SelectedIndex == -1)
            {
                //throw some error and exit
                NewRezept = null;
                return false;
            }
            NewRezept = new common.Rezept(nameIN.Text, gruppeIN.Text, laborIN.SelectedIndex, (propeIN_B.Value, propeIN_A.Value));
            NewRezept.Verbreitung = verbrIN.Text.Length != 0 ? verbrIN.Text : null;
            NewRezept.Haltbarkeit = haltbIN.Text.Length != 0 ? haltbIN.Text : null;
            NewRezept.Merkmale = merkmIN.Text.Length != 0 ? merkmIN.Text : null;
            NewRezept.Zutaten = zutatenIN.Text.Length != 0 ? zutatenIN.Text : null;
            NewRezept.Seite = seiteIN.Value;
            NewRezept.Beschaffung = beschIN_T.Text.Length != 0 ? Tuple.Create<string, int>(beschIN_T.Text, beschIN_V.Value) : null;
            NewRezept.Preis = preisIN.Text.Length != 0 ? preisIN.Text : null;
            if (wirkung != null)
            {
                NewRezept.Wirkung['M'] = wirkung[0];
                NewRezept.Wirkung['A'] = wirkung[1];
                NewRezept.Wirkung['B'] = wirkung[2];
                NewRezept.Wirkung['C'] = wirkung[3];
                NewRezept.Wirkung['D'] = wirkung[4];
                NewRezept.Wirkung['E'] = wirkung[5];
                NewRezept.Wirkung['F'] = wirkung[6];
            }
            return true;
        }

        public static RoutedCommand Exit_RoutedCommand = new RoutedCommand();
        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Get();
            this.Close();
        }
        public static RoutedCommand WirkungInput_RoutedCommand = new RoutedCommand();
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
