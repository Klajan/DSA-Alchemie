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
        public List<string> Groups { private set; get; }
        public Dictionary<string, List<string>> GroupDict { private set; get; }
        public Dictionary<string, common.Rezept> RezeptDict { private set; get; }
        private readonly string allKey = "Alle";

        public Database()
        {
            Groups = new List<string>();
            GroupDict = new Dictionary<string, List<string>>();
            RezeptDict = new Dictionary<string, common.Rezept>();
        }
        public void AddRezept(common.Rezept R)
        {
            try
            {
                RezeptDict.Add(R.Name, R);
            }
            catch(ArgumentException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
            }
        }
        public void CreateDictionary()
        {
            RezeptDict.Add(allKey, new common.Rezept(null, allKey, 0, (0, 0)));
            foreach(KeyValuePair<string, common.Rezept> re in RezeptDict)
            {
                if (!Groups.Contains(re.Value.Gruppe))
                {
                    GroupDict.Add(re.Value.Gruppe, null);
                    Groups.Add(re.Value.Gruppe);
                }
            }
            Groups.Remove(allKey);
            Groups.Sort();
            Groups.Insert(0, allKey);
            RezeptDict.Remove(allKey);
            foreach(string gr in Groups)
            {
                var list = new List<string>();
                foreach(KeyValuePair<string, common.Rezept> re in RezeptDict)
                {
                    if(gr == allKey)
                    {
                        list.Add(re.Value.Name);
                    }
                    else if(re.Value.Gruppe == gr)
                    {
                        list.Add(re.Value.Name);
                    }
                }
                list.Sort();
                GroupDict[gr] = list;
            }
        }
    }
}
