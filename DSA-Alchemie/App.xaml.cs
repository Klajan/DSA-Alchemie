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

        private Database rezepteDB_ = new Database();
        public Database RezepteDB { get { return rezepteDB_; } }
        private Character character_ = new Character();

        public Character Character
        {
            get => character_;
            set { character_ = value; }
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
                    rezepte = XmlHandler.ImportRezepteXml(xmlstream, xsdstream) as List<Rezept>;
                }
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
            MainWindow = new MainWindow();
            MainWindow.Activate();
            MainWindow.Show();
            var initTask = Task.Run(Initialize);
            var uiTask = initTask.ContinueWith(delegate
            {
                Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                    {
                        (MainWindow as MainWindow).AttachRezepte(rezepteDB_);
                        (MainWindow as MainWindow).AttachCharacter(character_);
                    }));
            }, TaskScheduler.Current);
        }

        public bool OpenAddRezeptWindow()
        {
            UI.Windows.InputRezeptWindow popup = new UI.Windows.InputRezeptWindow()
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
            if (Alchemie.Properties.Settings.Default.SaveCharacterOnExit)
            {
                Character.SaveCharacterToSettings();
                Alchemie.Properties.CharacterSave.Default.IsDefault = false;
                Alchemie.Properties.CharacterSave.Default.Save();
            }
        }
    }
}