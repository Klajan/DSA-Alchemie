﻿

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace DSA_Alchemie
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {
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

        private Database data_ = new Database();
        public Database Data { get { return data_; } }
        private Rezept currentRezept_;
        public Rezept CurrentRezept
        {
            get { return currentRezept_; }
            set { currentRezept_ = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentRezept")); }
        }
        private Trank trank_;
        public Trank Trank
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
            var xmlImport = Task.Run(() => XmlHandler.ImportXmlData("data", ref data_));
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
        public Rezept OpenAddRezeptWindow()
        {
            Window window = new AddRezeptWindow();
            window.DataContext = main;
            window.Show();
            return null;
        }
    }
}
