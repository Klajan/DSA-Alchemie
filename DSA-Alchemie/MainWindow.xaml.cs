using Alchemie.Models;
using Alchemie.UI.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Alchemie
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly App CurrentApp_;
        public App CurrentApp { get { return CurrentApp_; } }
        private readonly RezeptViewModel _rezeptModel = new();
        public RezeptViewModel RezeptModel { get { return _rezeptModel; } }

        public MainWindow()
        {
            CurrentApp_ = Application.Current as App;
            InitializeComponent();
        }

        public void AttachRezepte(Database data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            RezeptViewModel rezeptViewModel = new(CurrentApp.Trank);
            RezeptView.DataContext = rezeptViewModel;
            MainWindowViewModel mainWindowViewModel = new();
            MainGrid.DataContext = mainWindowViewModel;
            mainWindowViewModel.Attach_Rezepte(data);
            mainWindowViewModel.OnRezeptChanged += rezeptViewModel.ChangeRezept;
            BrauenView.BrauenViewModel.Trank = CurrentApp.Trank;
        }

        public void AttachCharacter(Character character)
        {
            CharacterViewMain.CharacterViewModel.Character = character;
            BrauenView.BrauenViewModel.Character = character;
        }

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