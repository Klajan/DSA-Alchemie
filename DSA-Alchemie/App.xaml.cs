

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using Alchemie.common;
using Alchemie.FileHandling;
using System.IO.Compression;

namespace Alchemie
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {
        public static List<Tuple<Exception, Type>> Exceptions = new List<Tuple<Exception, Type>>();
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        public static readonly Tuple<Int32, string>[] SubstitutionList = new Tuple<int, string>[]
        {
            Tuple.Create(-3, "(-3) Optimierende Substitution" ),
            Tuple.Create(0, "(+0) Gleichwertige Substitution"),
            Tuple.Create(+3, "(+3) Sinnvolle Substitution"),
            Tuple.Create(+3, "(+6) Mögliche Substitution"),
            Tuple.Create(+99, "Unsinnige Substitution")
        };
        public static readonly Tuple<Int32, string>[] LabQualityList = new Tuple<int, string>[]
        {
            Tuple.Create(+3, "(+3) Fehlende/beschädigte Gerätschaften"),
            Tuple.Create(0, "(+0) Normales Labor"),
            Tuple.Create(-3, "(-3) Hochwertiges Labor"),
            Tuple.Create(-7, "(-7) Außergewöhnlich hochwertiges Labor")
        };
        public static readonly Random rnd = new Random();

        private MainWindow main;

        private Database rezepte_ = new Database();
        public Database Rezepte { get { return rezepte_; } }
        private common.Character character_ = new Character();
        public Character Character
        {
            get => character_;
            set { character_ = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Character))); }
        }
        private common.Rezept currentRezept_;
        public common.Rezept CurrentRezept
        {
            get { return currentRezept_; }
            set { currentRezept_ = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentRezept))); }
        }
        private common.Trank trank_;
        public common.Trank Trank
        {
            get { return trank_; }
            set { trank_ = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Trank))); }
        }

        private void Initialize()
        {
            List<Rezept> rezepte = null;
            using (System.IO.Stream xml = EmbeddedHandling.GetEmbeddedRecourceStream("Alchemie.resources.data.rezepte.xml.deflate"),
                xsd = EmbeddedHandling.GetEmbeddedRecourceStream("Alchemie.resources.data.rezepte.xsd.deflate"))
            { using (System.IO.Stream xmlstream = new DeflateStream(xml, CompressionMode.Decompress), xsdstream = new DeflateStream(xsd, CompressionMode.Decompress)) {
                    rezepte = XmlHandler.ImportRezepteXml(xmlstream, xsdstream);
                }
            }
            rezepte_ = new Database(rezepte);
        }

        

        public App()
        {
            main = new MainWindow();
            MainWindow = main;
            main.Activate();
            main.Show();
            var initTask = Task.Run(Initialize);
            var uiTask = initTask.ContinueWith(delegate
            {
                Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate {
                        main.AttachRezepte(rezepte_);
#if DEBUG
                        XmlHandler.ExportRezepteToXml(rezepte_.GetList(), $"resources/data/export.xml");
#endif
                    }));
            });
        }
        public bool OpenAddRezeptWindow()
        {
            UI.Windows.InputRezeptWindow popup = new UI.Windows.InputRezeptWindow();
            popup.DataContext = main;
            popup.ShowDialog();
            var rezept = popup.NewRezept;
            if (rezept != null)
            {
                rezepte_.AddRezept(rezept);
                return true;
            }
            return false;
        }
    }
}
