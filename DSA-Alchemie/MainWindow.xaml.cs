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
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Threading;

namespace DSA_Alchemie
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> groups;
        public ObservableCollection<string> rezepte;
        readonly App app_;
        public App App { get { return app_; } }
        public MainWindow()
        {
            app_ = Application.Current as App;
            InitializeComponent();
            XmlHandler.mutex.WaitOne();
            XmlHandler.mutex.ReleaseMutex();
        }

        public void AttachRezepte(Database data)
        {
            groups = new ObservableCollection<string>(data.Groups);
            rezepte = new ObservableCollection<string>(data.GroupDict["Alle"]);
            rezepte_combo_group.ItemsSource = groups;
            rezepte_combo_rezept.ItemsSource = rezepte;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (null == rezepte_combo_rezept.SelectedItem) { return; }
            if(app_.Trank != null)
            {
                if (!app_.Trank.IsSameBase(app_.CurrentRezept))
                {
                    app_.Trank = new common.Trank(app_.CurrentRezept, app_.Trank.RollEign, app_.Trank.RollQual);
                }
            }
            else
            {
                app_.Trank = new common.Trank(app_.CurrentRezept);
            }
            app_.Trank.RNG = !ManDice.IsChecked;
            int mod = ((Tuple<int, string>)brauen_combo_substi.SelectedItem).Item1 + ((Tuple<int, string>)LaborQuality.SelectedItem).Item1;
            var quality = app_.Trank.Brauen(mod, (brauen_input_rckHalten.Value, brauen_input_astralAuf.Value, 0), app_.Character);
            brauen_txtBox_quality.Text = quality.ToString();
        }

        private void ComboBoxRezepteGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grouped = app_.Data.GroupDict[rezepte_combo_group.SelectedItem.ToString()];
            rezepte.Clear();
            foreach(string st in grouped)
            {
                rezepte.Add(st);
            }
            rezepte_combo_rezept.SelectedIndex = 0;
        }
        private void ComboBoxRezepteRezept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(null == rezepte_combo_rezept.SelectedItem) { return; }
            app_.CurrentRezept = app_.Data.RezeptDict[rezepte_combo_rezept.SelectedItem.ToString()];
            bool same = (app_.Trank != null) ? app_.Trank.IsSameBase(app_.CurrentRezept) : false;
            if (!same) { app_.Trank = new common.Trank(app_.CurrentRezept); }
            brauen_txtBox_quality.Text = "";
        }

        Regex qualRegex = new Regex("(?i)[MABCDEF]");
        private void BrauenQuality_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox origin = sender as TextBox;
            e.Handled = true;
            var match = qualRegex.Match(origin.Text);
            if (app_.CurrentRezept == null) return;
            if (app_.Trank != null)
            {
                if (match.Success && !app_.Trank.IsSameBase(app_.CurrentRezept))
                {
                    app_.Trank = new common.Trank(app_.CurrentRezept, app_.Trank.RollEign, app_.Trank.RollQual);
                }
            }
            else
            {
                app_.Trank = new common.Trank(app_.CurrentRezept);
            }
            app_.Trank.Quality = match.Success ? match.Value[0] : '-';
            origin.TextChanged -= BrauenQuality_TextChanged;
            origin.Text = match.Value.ToUpper();
            origin.TextChanged += BrauenQuality_TextChanged;
        }
        public static RoutedCommand AddRezept_RoutedCommand = new RoutedCommand();
        private void AddRezeptCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddRezeptCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            app_.OpenAddRezeptWindow();
            AttachRezepte(app_.Data);
        }
    }
}