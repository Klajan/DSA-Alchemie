using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using Alchemie;
using Alchemie.common;

namespace ParseRezeptString
{
    class Program
    {
        static string NormalizeString (string input)
        {
            return Regex.Replace(Regex.Replace(input, @"\r\n?|\n|\\t", ""), @"\s+", " ").Trim();
        }
        static Regex RezeptRegex = new Regex(@"^(?'rezept'.+)Gruppe:(?'gruppe'.+)Beschaffung:(?'beschaffung'.+)Rezeptur:(?'rezeptur'.+)(?'beschreibung'».+)?Labor:(?'labor'.+)Probe:(?'probe'.+)Wirkung:(?'wirkung'.+)Verbreitung:(?'verbreitung'.+)Merkmale:(?'merkmale'.+)Haltbarkeit:(?'haltbarkeit'.+)Preis:(?'preis'.+)Meisterhinweise:(?'meisterhinweise'.+)$");
        static void Main(string[] args)
        {
            if (args.Length > 1) return;
            string filepath = args[0];
            if (!File.Exists(filepath)) return;
            TextReader reader = new StreamReader(filepath);
            XPathDocument document = new XPathDocument(reader);
            var NodeIterator = document.CreateNavigator().Select("strings/string");
            List<Match> matches = new List<Match>();
            while(NodeIterator.MoveNext())
            {
                matches.Add(RezeptRegex.Match(Program.NormalizeString(NodeIterator.Current.Value)));
            }
            List<Rezept> rezepte = new List<Rezept>();
            foreach(var match in matches)
            {
                if(match.Success)
                {
                    var probe = match.Groups["probe"].ToString().Trim().Split(@"/");
                    var rezept = new Rezept(
                        match.Groups["rezept"].ToString().Trim(),
                        match.Groups["gruppe"].ToString().Trim(),
                        match.Groups["labor"].ToString().Trim(),
                        (int.Parse(Regex.Replace(probe[0].Trim(), @"\s+", "")), int.Parse(Regex.Replace(probe[1].Trim(), @"\s+", "")))
                        );
                    rezept.Rezeptur = match.Groups["rezeptur"].ToString().Trim();
                    rezept.Beschreibung = match.Groups["beschreibung"]?.ToString().Trim();
                    rezept.Verbreitung = match.Groups["verbreitung"].ToString().Trim();
                    rezept.Merkmale = match.Groups["merkmale"].ToString().Trim();
                    rezept.Haltbarkeit = match.Groups["haltbarkeit"].ToString().Trim();
                    rezept.Preis = match.Groups["preis"].ToString().Trim();
                    rezept.Meisterhinweise = match.Groups["meisterhinweise"].ToString().Trim();
                    var beschaffung = match.Groups["beschaffung"].ToString().Trim().Split(@"/");
                    rezept.Beschaffung = Tuple.Create(beschaffung[0].Trim(), int.Parse(beschaffung[1].Trim()));
                    var wirkung = Regex.Match(match.Groups["wirkung"].ToString().Trim(), @"^M:(?'M'.+)A:(?'A'.+)B:(?'B'.+)C:(?'C'.+)D:(?'D'.+)E:(?'E'.+)F:(?'F'.+)$");
                    char[] CA = { 'M', 'A', 'B', 'C', 'D', 'E', 'F' };
                    if (wirkung.Success)
                    {
                        foreach (var c in CA)
                        {
                            rezept.Wirkung[c] = wirkung.Groups[c.ToString()].ToString().Trim(); ;
                        }
                    }
                    rezepte.Add(rezept);
                }
            }
            Console.WriteLine($"Exporting {rezepte.Count} Rezepte");
            XmlHandler.ExportRezepteToXml(rezepte, @"export.xml");
        }
    }
}
