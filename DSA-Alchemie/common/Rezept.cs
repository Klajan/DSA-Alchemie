using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DSA_Alchemie.common
{
    public class Rezept : NotifyPropertyChanged
    {
        private static UInt64 lastID = 0; 
        public readonly UInt64 ID;
        protected readonly bool isValid;
        public string Name { private set; get; }
        public string Gruppe { private set; get; }
        /// <summary>
        /// Item1:  Numeric Identifier <para/>
        /// Item2:  Display Name
        /// </summary>
        public Tuple<int, string> Labor { private set; get; }
        /// <summary>
        /// Item1:  Mod für Brauen Probe <para/>
        /// Item2:  Mod für Analyse Probe
        /// </summary>
        public Tuple<int, int> Probe { private set; get; }
        public string Verbreitung { set; get; }
        public string Haltbarkeit { set; get; }
        /// <summary>
        /// Item1:  Preis der Zutaten als string <para/>
        /// Item2:  Verbreitung der Zutaten als int
        /// </summary>
        public Tuple<string, int> Beschaffung { set; get; }
        public string Preis { set; get; }
        public string Rezeptur { set; get; }
        public int Seite { set; get; }
        public string Merkmale { set; get; }
        public string Beschreibung { set; get; }
        public string Meisterhinweise { set; get; }
        public Dictionary<char, string> Wirkung { private set; get; }
        public string this[char key]
        {
            get => Wirkung[key];
            set => Wirkung[key] = value;
        }
        public Rezept() { isValid = false; }
        public Rezept(string name, string group, string labor, (int brauen, int analyse) probe)
        {
            ID = lastID++;
            this.Name = name; this.Gruppe = group; this.Probe = probe.ToTuple<int, int>();
            switch (labor)
            {
                case "0":
                case "archaisches Labor": 
                    this.Labor = Tuple.Create(0, "archaisches Labor");
                    break;
                case "1":
                case "Hexenküche":
                    this.Labor = Tuple.Create(1, "Hexenküche");
                    break;
                case "2":
                case "Alchimistenlabor":
                    this.Labor = Tuple.Create(2, "Alchimistenlabor");
                    break;
                default:
                    this.Labor = Tuple.Create(-1, "Unbekannt");
                    break;
            }
            Wirkung = new Dictionary<char, string>();
            isValid = true;
        }
        public Rezept(Rezept prevRezept)
        {
            this.ID = prevRezept.ID;
            this.Name = prevRezept.Name;
            this.Gruppe = prevRezept.Gruppe;
            this.Labor = prevRezept.Labor;
            this.Probe = prevRezept.Probe;
            this.Verbreitung = prevRezept.Verbreitung;
            this.Haltbarkeit = prevRezept.Haltbarkeit;
            this.Beschaffung = prevRezept.Beschaffung;
            this.Preis = prevRezept.Preis;
            this.Wirkung = new Dictionary<char, string>(prevRezept.Wirkung);
            this.Merkmale = prevRezept.Merkmale;
            this.Seite = prevRezept.Seite;
            this.isValid = prevRezept.isValid;
        }
    }
}
