using Alchemie.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alchemie
{
    public class Database
    {
        private readonly List<string> _gruppen;

        public IReadOnlyList<string> Gruppen
        {
            get { return _gruppen; }
        }

        //public List<string> Gruppen { private set; get; }

        private readonly Dictionary<string, IReadOnlyList<string>> _rezepteGruppen;

        public IReadOnlyDictionary<string, IReadOnlyList<string>> RezepteGruppen
        {
            get { return _rezepteGruppen; }
        }

        private readonly Dictionary<string, Rezept> _rezepte;

        public IReadOnlyDictionary<string, Rezept> Rezepte
        {
            get { return _rezepte; }
        }

        public string AllKey { get; } = "Alle";

        public Database()
        {
            _gruppen = new List<string>();
            _rezepteGruppen = new Dictionary<string, IReadOnlyList<string>>();
            _rezepte = new Dictionary<string, Rezept>();
        }

        public Database(IList<Rezept> rezepte)
        {
            _rezepteGruppen = new Dictionary<string, IReadOnlyList<string>>();
            _rezepte = rezepte.ToDictionary((Rezept x) => x.Name, (Rezept x) => x);
            _gruppen = Rezepte.Values.Select(x => x.Gruppe).Distinct().ToList();
            foreach (var gruppe in Gruppen)
            {
                _rezepteGruppen.Add(gruppe, Rezepte.Values.Where(x => x.Gruppe == gruppe).Select(x => x.Name).ToList());
            }
            _gruppen.Insert(0, AllKey);
            _rezepteGruppen.Add(AllKey, Rezepte.Values.Select(x => x.Name).ToList());
        }

        public void AddRezept(Rezept rezept)
        {
            try
            {
                if (rezept == null) throw new ArgumentNullException(nameof(rezept));
                _rezepte.Add(rezept.Name, rezept);
            }
            catch (ArgumentException e)
            {
                System.Windows.MessageBox.Show(e.Message, "ArgumentNullException");
                return;
            }
            if (RezepteGruppen.ContainsKey(rezept.Gruppe))
            {
                (RezepteGruppen[rezept.Gruppe] as IList<string>).Add(rezept.Name);
            }
            else
            {
                _gruppen.Add(rezept.Gruppe);
                _rezepteGruppen.Add(rezept.Gruppe, new List<string> { rezept.Name });
            }
            (RezepteGruppen[AllKey] as IList<string>).Add(rezept.Name);
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