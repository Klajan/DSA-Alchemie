using Alchemie.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alchemie
{
    public class Database
    {
        public List<string> Gruppen { private set; get; }
        public Dictionary<string, List<string>> RezepteGruppen { private set; get; }
        public Dictionary<string, Rezept> Rezepte { private set; get; }
        public string AllKey { get; } = "Alle";

        public Database()
        {
            Gruppen = new List<string>();
            RezepteGruppen = new Dictionary<string, List<string>>();
            Rezepte = new Dictionary<string, Rezept>();
        }

        public Database(List<Rezept> rezepte)
        {
            RezepteGruppen = new Dictionary<string, List<string>>();
            Rezepte = rezepte.ToDictionary((Rezept x) => x.Name, (Rezept x) => x);
            Gruppen = Rezepte.Values.Select(x => x.Gruppe).Distinct().ToList();
            foreach (var gruppe in Gruppen)
            {
                RezepteGruppen.Add(gruppe, Rezepte.Values.Where(x => x.Gruppe == gruppe).Select(x => x.Name).ToList());
            }
            Gruppen.Insert(0, AllKey);
            RezepteGruppen.Add(AllKey, Rezepte.Values.Select(x => x.Name).ToList());
        }

        public void AddRezept(Rezept rezept)
        {
            try
            {
                if (rezept == null) throw new ArgumentNullException(nameof(rezept));
                Rezepte.Add(rezept.Name, rezept);
            }
            catch (ArgumentException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                return;
            }
            if (RezepteGruppen.ContainsKey(rezept.Gruppe))
            {
                RezepteGruppen[rezept.Gruppe].Add(rezept.Name);
            }
            else
            {
                Gruppen.Add(rezept.Gruppe);
                RezepteGruppen.Add(rezept.Gruppe, new List<string> { rezept.Name });
            }
            RezepteGruppen[AllKey].Add(rezept.Name);
        }

        public List<Rezept> GetList()
        {
            return Rezepte.Values.ToList();
        }

        public Rezept this[string key]
        {
            get
            {
                return Rezepte.ContainsKey(key) ? Rezepte[key] : null;
            }
        }
    }
}