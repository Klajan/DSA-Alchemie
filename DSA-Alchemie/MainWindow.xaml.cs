﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Threading;
using Alchemie.UI.ViewModels;
using Alchemie.common;

namespace Alchemie
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal ObservableCollection<string> groups;
        internal ObservableCollection<string> rezepte;
        private readonly App CurrentApp_;
        public  App CurrentApp { get { return CurrentApp_; } }
        RezeptViewModel _rezeptModel = new RezeptViewModel();
        public RezeptViewModel RezeptModel { get { return _rezeptModel; } }
        public MainWindow()
        {
            CurrentApp_ = Application.Current as App;
            InitializeComponent();
        }

        public void AttachRezepte(Database data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            groups = new ObservableCollection<string>(data.Gruppen);
            rezepte = new ObservableCollection<string>(data.RezepteGruppen["Alle"]);
            rezepte_combo_group.ItemsSource = groups;
            rezepte_combo_rezept.ItemsSource = rezepte;
            RezeptView.DataContext = _rezeptModel;
        }

        public void AttachCharacter(Character character)
        {
            CharacterViewMain.CharacterViewModel.Character = character;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (null == rezepte_combo_rezept.SelectedItem) { return; }
            if(CurrentApp.Trank != null)
            {
                if (!CurrentApp.Trank.IsSameBase(CurrentApp.CurrentRezept))
                {
                    CurrentApp.Trank = new common.Trank(CurrentApp.CurrentRezept, CurrentApp.Trank.EigenschaftDice.ToList(), CurrentApp.Trank.QualityDice.ToList());
                }
            }
            else
            {
                CurrentApp.Trank = new common.Trank(CurrentApp.CurrentRezept);
                trankViewModel_.Trank = CurrentApp.Trank;
            }
            CurrentApp.Trank.RNG = !ManDice.IsChecked;
            int mod = ((Tuple<int, string>)brauen_combo_substi.SelectedItem).Item1 + (int)CharacterViewMain.CharacterViewModel.LaborQuality;
            var quality = trankViewModel_.Trank.Brauen(mod, (brauen_input_rckHalten.Value, brauen_input_astralAuf.Value, 0), CharacterViewMain.CharacterViewModel.Character);
            brauen_txtBox_quality.Text = quality.ToString(CultureInfo.CurrentCulture);
        }

        private void ComboBoxRezepteGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grouped = CurrentApp.Rezepte.RezepteGruppen[rezepte_combo_group.SelectedItem.ToString()];
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
            CurrentApp.CurrentRezept = CurrentApp.Rezepte[rezepte_combo_rezept.SelectedItem.ToString()];
            if (!((CurrentApp.Trank != null) && CurrentApp.Trank.IsSameBase(CurrentApp.CurrentRezept))) {
                CurrentApp.Trank = new common.Trank(CurrentApp.CurrentRezept);
                 _rezeptModel.Rezept = CurrentApp.Rezepte[rezepte_combo_rezept.SelectedItem.ToString()];
                trankViewModel_.Trank = CurrentApp.Trank;
            }
            brauen_txtBox_quality.Text = "";
        }

        private readonly Regex qualRegex = new Regex("(?i)[MABCDEF]");
        private void BrauenQuality_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox origin = sender as TextBox;
            e.Handled = true;
            var match = qualRegex.Match(origin.Text);
            if (CurrentApp.CurrentRezept == null) return;
            if (CurrentApp.Trank != null)
            {
                if (match.Success && !CurrentApp.Trank.IsSameBase(CurrentApp.CurrentRezept))
                {
                    CurrentApp.Trank = new common.Trank(CurrentApp.CurrentRezept, CurrentApp.Trank.EigenschaftDice.ToList(), CurrentApp.Trank.QualityDice.ToList());
                }
            }
            else
            {
                CurrentApp.Trank = new common.Trank(CurrentApp.CurrentRezept);
            }
            trankViewModel_.Quality = match.Success ? match.Value[0] : '-';
            origin.TextChanged -= BrauenQuality_TextChanged;
            origin.Text = match.Value.ToUpper(CultureInfo.CurrentCulture);
            origin.TextChanged += BrauenQuality_TextChanged;
        }
        public static RoutedCommand AddRezeptRoutedCommand { set; get; } = new RoutedCommand();
        private void AddRezeptCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddRezeptCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentApp.OpenAddRezeptWindow();
            AttachRezepte(CurrentApp.Rezepte);
        }

        private void Brauen_input_rckHalten_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}