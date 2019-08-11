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
        public Dictionary<string, Rezept> RezeptDict { private set; get; }
        private readonly string allKey = "Alle";

        public Database()
        {
            Groups = new List<string>();
            GroupDict = new Dictionary<string, List<string>>();
            RezeptDict = new Dictionary<string, Rezept>();
        }
        public void AddRezept(Rezept R)
        {
            RezeptDict.Add(R.Name, R);
        }
        public void CreateDictionary()
        {
            GroupDict.Add(allKey, null);
            Groups.Add(allKey);
            foreach(KeyValuePair<string, Rezept> re in RezeptDict)
            {
                if (!GroupDict.ContainsKey(re.Value.Group))
                {
                    GroupDict.Add(re.Value.Group, null);
                    Groups.Add(re.Value.Group);
                }
            }
            var tmpDict = new Dictionary<string, List<string>>(GroupDict);
            foreach(KeyValuePair<string, List<string>> gr in GroupDict)
            {
                var list = new List<string>();
                foreach(KeyValuePair<string, Rezept> re in RezeptDict)
                {
                    if(gr.Key == allKey)
                    {
                        list.Add(re.Value.Name);
                    }
                    else if(re.Value.Group == gr.Key)
                    {
                        list.Add(re.Value.Name);
                    }
                }
                list.Sort();
                tmpDict[gr.Key] = list;
            }
            GroupDict = new Dictionary<string, List<string>>(tmpDict);
        }
    }
}
