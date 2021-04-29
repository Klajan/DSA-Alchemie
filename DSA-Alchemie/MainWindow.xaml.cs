using Alchemie.Models;
using Alchemie.UI.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Alchemie
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //internal ObservableCollection<string> groups;
        //internal ObservableCollection<string> rezepte;
        private readonly App CurrentApp_;
        public App CurrentApp { get { return CurrentApp_; } }
        RezeptViewModel _rezeptModel = new RezeptViewModel();
        public RezeptViewModel RezeptModel { get { return _rezeptModel; } }
        public MainWindow()
        {
            CurrentApp_ = Application.Current as App;
            InitializeComponent();
        }

        public void AttachRezepte(Database data)
        {
            RezeptViewModel rezeptViewModel = new RezeptViewModel(CurrentApp.Trank);
            RezeptView.DataContext = rezeptViewModel;
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;
            mainWindowViewModel.Attach_Rezepte(data);
            mainWindowViewModel.OnRezeptChanged += rezeptViewModel.ChangeRezept;
            BrauenView.BrauenViewModel.Trank = CurrentApp.Trank;
            
            /*
            if (data == null) throw new ArgumentNullException(nameof(data));
            groups = new ObservableCollection<string>(data.Gruppen);
            rezepte = new ObservableCollection<string>(data.RezepteGruppen["Alle"]);
            rezepte_combo_group.ItemsSource = groups;
            rezepte_combo_rezept.ItemsSource = rezepte;
            BrauenView.BrauenViewModel.Trank = CurrentApp.Trank;
            */
        }

        public void AttachCharacter(Character character)
        {
            CharacterViewMain.CharacterViewModel.Character = character;
            BrauenView.BrauenViewModel.Character = CurrentApp.Character;
        }
        /*
        private void ComboBoxRezepteGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grouped = CurrentApp.RezepteDB.RezepteGruppen[rezepte_combo_group.SelectedItem.ToString()];
            rezepte.Clear();
            foreach (string st in grouped)
            {
                rezepte.Add(st);
            }
            rezepte_combo_rezept.SelectedIndex = 0;
        }
        private void ComboBoxRezepteRezept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null == rezepte_combo_rezept.SelectedItem) { return; }
            Rezept rezept = CurrentApp.RezepteDB[rezepte_combo_rezept.SelectedItem.ToString()];
            if (CurrentApp.Trank != null && !CurrentApp.Trank.IsSameBase(rezept))
            {
                CurrentApp.Trank.Rezept = rezept;
            }
        }
        */
        public static RoutedCommand AddRezeptRoutedCommand { set; get; } = new RoutedCommand();
        private void AddRezeptCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddRezeptCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentApp.OpenAddRezeptWindow();
            AttachRezepte(CurrentApp.RezepteDB);
        }
    }
}