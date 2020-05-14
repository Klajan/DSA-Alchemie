using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace DSA_Alchemie
{
    public class Database
    {
        public List<string> Gruppen { private set; get; }
        public Dictionary<string, List<string>> RezepteGruppen { private set; get; }
        public Dictionary<string, common.Rezept> Rezepte { private set; get; }
        private readonly static string allKey = "Alle";

        public Database()
        {
            Gruppen = new List<string>();
            RezepteGruppen = new Dictionary<string, List<string>>();
            Rezepte = new Dictionary<string, common.Rezept>();
        }
        public Database(List<common.Rezept> rezepte)
        {
            RezepteGruppen = new Dictionary<string, List<string>>();
            Rezepte = rezepte.ToDictionary((common.Rezept x) => x.Name, (common.Rezept x) => x);
            Gruppen = Rezepte.Values.Select(x => x.Gruppe).Distinct().ToList();
            foreach(var gruppe in Gruppen)
            {
                RezepteGruppen.Add(gruppe, Rezepte.Values.Where(x => x.Gruppe == gruppe).Select(x => x.Name).ToList());
            }
            Gruppen.Insert(0, allKey);
            RezepteGruppen.Add(allKey, Rezepte.Values.Select(x => x.Name).ToList());
        }
        public void AddRezept(common.Rezept rezept)
        {
            try
            {
                Rezepte.Add(rezept.Name, rezept);
            }
            catch(ArgumentException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                return;
            }
            if(RezepteGruppen.ContainsKey(rezept.Gruppe))
            {
                RezepteGruppen[rezept.Gruppe].Add(rezept.Name);
            } else
            {
                Gruppen.Add(rezept.Gruppe);
                RezepteGruppen.Add(rezept.Gruppe, new List<string> { rezept.Name });
            }
        }
        public List<common.Rezept> GetList()
        {
            return Rezepte.Values.ToList();
        }
    }
}
