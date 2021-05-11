using System;
using System.Text.RegularExpressions;
using Alchemie.Models.Types;

namespace Alchemie.Models
{
    public class Rezept : IEquatable<Rezept>
    {
        private static uint lastID = 1;
        internal uint ID { private set; get; }
        internal bool IsValid { get => ID != 0; }
        public string Name { private set; get; }
        public string Gruppe { private set; get; }
        public Labor Labor { private set; get; }
        public Probe Probe { private set; get; }
        public string Verbreitung { set; get; }
        public Haltbarkeit Haltbarkeit { set; get; }
        public Beschaffung Beschaffung { set; get; }
        public string Preis { set; get; }
        public string Rezeptur { set; get; }
        public int Seite { set; get; }
        public string Merkmale { set; get; }
        public string Beschreibung { set; get; }
        public string Meisterhinweise { set; get; }
        public Wirkung Wirkung { set; get; }

        public Rezept()
        {
        }

        public Rezept(string name, string group, string labor, (int brauen, int analyse) probe)
        {
            ID = lastID++;
            Name = name;
            Gruppe = group;
            Probe = new Probe(probe.brauen, probe.analyse);
            Labor = new Labor(labor);
        }

        public Rezept(Rezept prevRezept)
        {
            if (prevRezept != null && prevRezept.IsValid)
            {
                ID = lastID++;
                Name = prevRezept.Name;
                Gruppe = prevRezept.Gruppe;
                Labor = prevRezept.Labor;
                Probe = prevRezept.Probe;
                Verbreitung = prevRezept.Verbreitung;
                Haltbarkeit = prevRezept.Haltbarkeit;
                Beschaffung = prevRezept.Beschaffung;
                Preis = prevRezept.Preis;
                Wirkung = prevRezept.Wirkung;
                Merkmale = prevRezept.Merkmale;
                Seite = prevRezept.Seite;
                Rezeptur = prevRezept.Rezeptur;
                Beschreibung = prevRezept.Beschreibung;
                Meisterhinweise = prevRezept.Meisterhinweise;
            }
        }

        public bool Equals(Rezept other)
        {
            return other != null &&
                   IsValid & other.IsValid &&
                   ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            return obj is Rezept rezept && Equals(rezept);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}