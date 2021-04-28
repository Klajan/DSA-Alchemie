

using Alchemie.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Alchemie
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public static List<Tuple<Exception, Type>> Exceptions { private set; get; } = new List<Tuple<Exception, Type>>();

        private readonly MainWindow main;

       private Database rezepte_ = new Database();
        public Database Rezepte { get { return rezepte_; } }
        private Character character_ = new Character();
        public Character Character
        {
            get => character_;
            set { character_ = value; }
        }
        private Rezept currentRezept_ = new Rezept();
        public Rezept CurrentRezept
        {
            get { return currentRezept_; }
            set { currentRezept_ = value; }
        }
        private Trank trank_ = new Trank();
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
                using (System.IO.Stream xmlstream = new DeflateStream(compressedXml, CompressionMode.Decompress), xsdstream = new DeflateStream(compressedXsd, CompressionMode.Decompress))
                {
                    rezepte = XmlHandler.ImportRezepteXml(xmlstream, xsdstream);
                }
            }
            rezepte_ = new Database(rezepte);

            if (!Alchemie.Properties.CharacterSave.Default.IsDefault)
            {
                character_ = Character.LoadCharacterFromSettings();
            }
            currentRezept_ = rezepte_.Rezepte.First().Value;
            trank_ = new Trank(currentRezept_, character_);
        }



        public App()
        {
            InitializeComponent();
            main = new MainWindow();
            MainWindow = main;
            main.Activate();
            main.Show();
            var initTask = Task.Run(Initialize);
            var uiTask = initTask.ContinueWith(delegate
            {
                Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                    {
                        main.AttachRezepte(rezepte_);
                        main.AttachCharacter(character_);
                    }));
            }, TaskScheduler.Current);
        }
        public bool OpenAddRezeptWindow()
        {
            UI.Windows.InputRezeptWindow popup = new UI.Windows.InputRezeptWindow()
            {
                DataContext = main
            };
            popup.ShowDialog();
            var rezept = popup.NewRezept;
            if (rezept != null)
            {
                rezepte_.AddRezept(rezept);
                return true;
            }
            return false;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Character.SaveCharacterToSettings();
            Alchemie.Properties.CharacterSave.Default.IsDefault = false;
            Alchemie.Properties.CharacterSave.Default.Save();
        }
    }
}
