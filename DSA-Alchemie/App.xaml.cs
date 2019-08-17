

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using DSA_Alchemie.dataClasses;

namespace DSA_Alchemie
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
        public static Tuple<Int32, string>[] SubstitutionList = new Tuple<int, string>[]
        {
            Tuple.Create(-3, "(-3) Optimierende Substitution" ),
            Tuple.Create(0, "(+0) Gleichwertige Substitution"),
            Tuple.Create(+3, "(+3) Sinnvolle Substitution"),
            Tuple.Create(+3, "(+6) Mögliche Substitution"),
            Tuple.Create(+99, "Unsinnige Substitution")
        };
        public static Tuple<Int32, string>[] LabQualityList = new Tuple<int, string>[]
        {
            Tuple.Create(+3, "(+3) Fehlende/beschädigte Gerätschaften"),
            Tuple.Create(0, "(+0) Normales Labor"),
            Tuple.Create(-3, "(-3) Hochwertiges Labor"),
            Tuple.Create(-7, "(-7) Außergewöhnlich hochwertiges Labor")
        };
        public static Random rnd = new Random();

        private MainWindow main;

        private dataClasses.Database data_ = new dataClasses.Database();
        public dataClasses.Database Data { get { return data_; } }
        private dataClasses.Character character_ = new Character();
        public Character Character
        {
            get => character_;
            set { character_ = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Character")); }
        }
        private dataClasses.Rezept currentRezept_;
        public dataClasses.Rezept CurrentRezept
        {
            get { return currentRezept_; }
            set { currentRezept_ = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentRezept")); }
        }
        private dataClasses.Trank trank_;
        public dataClasses.Trank Trank
        {
            get { return trank_; }
            set { trank_ = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Trank")); }
        }

        

        public App()
        {
            /*Window progress = new Window();
            progress.Show();
            progress.Activate();*/
            //Task xmlImport = new Task(XmlHandler.ImportXmlData("data", ref data_));
            var xmlImport = Task.Run(() => XmlHandler.ImportXmlData($"data/data.xml", ref data_));
            //XmlHandler.ImportXmlData("data", ref data_);
            main = new MainWindow(this);
            MainWindow = main;
            main.Activate();
            main.Show();
            xmlImport.Wait();
            xmlImport.Dispose();
            data_.CreateDictionary();
            main.AttachRezepte(data_);
        }
        public bool OpenAddRezeptWindow()
        {
            window.InputRezeptWindow popup = new window.InputRezeptWindow();
            popup.DataContext = main;
            popup.ShowDialog();
            var rezept = popup.NewRezept;
            if (rezept != null)
            {
                data_.AddRezept(rezept);
                data_.CreateDictionary();
                return true;
            }
            return false;
        }
    }
}
