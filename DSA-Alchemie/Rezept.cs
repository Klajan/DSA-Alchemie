using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_Alchemie
{
    public class Rezept : NotifyPropertyChanged
    {
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
        public Tuple<int, int> Mods { private set; get; }
        public string Verbreitung { set; get; }
        public string Haltbarkeit { set; get; }
        /// <summary>
        /// Item1:  Preis der Zutaten als string <para/>
        /// Item2:  Verbreitung der Zutaten als int
        /// </summary>
        public Tuple<string, int> Beschaffung { set; get; }
        public string Preis { set; get; }
        public string Zutaten { set; get; }
        public int Seite { set; get; }
        public string Merkmale { set; get; }
        public Dictionary<char, string> Wirkung { private set; get; }
        public string this[char key]
        {
            get => Wirkung[key];
            set => Wirkung[key] = value;
        }
        public Rezept() { isValid = false; }
        public Rezept(string name, string group, int labor, (int brauen, int analyse) probe)
        {
            this.Name = name; this.Gruppe = group; this.Mods = probe.ToTuple<int, int>();
            switch (labor)
            {
                case 0:
                    this.Labor = Tuple.Create(0, "Archaisches Labor");
                    break;
                case 1:
                    this.Labor = Tuple.Create(1, "Hexenküche");
                    break;
                case 2:
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
            this.Name = prevRezept.Name;
            this.Gruppe = prevRezept.Gruppe;
            this.Labor = prevRezept.Labor;
            this.Mods = prevRezept.Mods;
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
