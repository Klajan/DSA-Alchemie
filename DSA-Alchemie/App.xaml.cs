using Alchemie.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

[assembly: CLSCompliant(true)]

namespace Alchemie
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public static Collection<Tuple<Exception, Type>> Exceptions { private set; get; } = new Collection<Tuple<Exception, Type>>();

        private Database rezepteDB_ = new();
        public Database RezepteDB { get { return rezepteDB_; } }
        private Character character_ = new();

        public Character Character
        {
            get => character_;
            set { character_ = value; }
        }

        private Trank trank_ = new();

        public Trank Trank
        {
            get { return trank_; }
            set { trank_ = value; }
        }

        private void Initialize()
        {
            List<Rezept> rezepte = null;
            using (System.IO.Stream compressedXml = new System.IO.MemoryStream(Alchemie.Resources.Data.rezepte_xml),
                compressedXsd = new System.IO.MemoryStream(Alchemie.Resources.Data.rezepte_xsd))
            {
                using System.IO.Stream xmlstream = new DeflateStream(compressedXml, CompressionMode.Decompress), xsdstream = new DeflateStream(compressedXsd, CompressionMode.Decompress);
                rezepte = XmlHandler.ImportRezepteXml(xmlstream, xsdstream) as List<Rezept>;
            }
            rezepteDB_ = new Database(rezepte);

            if (Alchemie.Properties.Settings.Default.LoadCharacterOnStart)
            {
                character_ = Character.LoadCharacterFromSettings();
            }
            trank_ = new Trank(rezepteDB_.Rezepte.First().Value, character_);
        }

        public App()
        {
            InitializeComponent();
            if (Alchemie.Properties.Settings.Default.UpgradeRequired)
            {
                Alchemie.Properties.Settings.Default.Upgrade();
                Alchemie.Properties.CharacterSave.Default.Upgrade();
                Alchemie.Properties.Settings.Default.UpgradeRequired = false;
                Alchemie.Properties.Settings.Default.Save();
            }
            MainWindow = new MainWindow();
            MainWindow.Activate();
            MainWindow.Show();

            var initTask = Task.Run(Initialize);
            initTask.ContinueWith(delegate
            {
                Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                    {
                        (MainWindow as MainWindow).AttachRezepte(rezepteDB_);
                        (MainWindow as MainWindow).AttachCharacter(character_);
                    }));
            }, TaskScheduler.Current);

            var updateChecker = Task.Run(UpdateChecker.CheckUpdateAvailable);
            updateChecker.ContinueWith(UpdaterWindow, TaskScheduler.Current);
        }

        private static void UpdaterWindow(Task<Release> release)
        {
            var result = release.Result;
            if (result == null) return;
            Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(delegate
                    {
                        UI.Windows.UpdateWindow UpdateWindow = new(result);
                        UpdateWindow.Show();
                    }));
        }

        public bool OpenAddRezeptWindow()
        {
            UI.Windows.InputRezeptWindow popup = new()
            {
                DataContext = (MainWindow as MainWindow)
            };
            popup.ShowDialog();
            var rezept = popup.NewRezept;
            if (rezept != null)
            {
                rezepteDB_.AddRezept(rezept);
                return true;
            }
            return false;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Alchemie.Properties.Settings.Default.Save();
            if (Alchemie.Properties.Settings.Default.SaveCharacterOnExit)
            {
                Character.SaveCharacterToSettings();
                Alchemie.Properties.CharacterSave.Default.IsDefault = false;
                Alchemie.Properties.CharacterSave.Default.Save();
            }
        }
    }
}