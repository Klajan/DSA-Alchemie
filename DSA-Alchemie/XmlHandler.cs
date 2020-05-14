using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using System.Xml.Serialization;


namespace DSA_Alchemie
{
    public class XmlHandler
    {
        static public Mutex ReaderMutex = new Mutex();
        static private string NormalizeStr(string input)
        {
            if (input != null)
            {
                return Regex.Replace(Regex.Replace(input, @"\r\n?|\n", ""), @"\s+", " ").Trim();
            }
            return input;
        }
        static private XmlSchema GetSchema(Stream xsdStream)
        {
            void ValidationCallBack(object sender, ValidationEventArgs args)
            {
                var test = args;
                return;
            }
            return XmlSchema.Read(
                xsdStream,
                ValidationCallBack);
        }
        static public List<common.Rezept> ImportRezepteXml(Stream xmlStream, Stream xsdStream = null)
        {
            lock (xmlStream)
            {
                List<common.Rezept> rezepte = new List<common.Rezept>();
                XmlReader reader = null;
                XmlSchemaSet schemaSet = new XmlSchemaSet(); ;
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                void cleanup()
                {
                    if (reader != null) { reader.Close(); }
                }
                XPathDocument doc;
                try
                {
                    if (xsdStream != null)
                    {
                        schemaSet.Add(GetSchema(xsdStream));
                        schemaSet.Compile();
                        readerSettings.ValidationType = ValidationType.Schema;
                        readerSettings.Schemas = schemaSet;

                    }
                    reader = XmlReader.Create(xmlStream, readerSettings);
                    doc = new XPathDocument(reader, XmlSpace.Preserve);
                }
                catch (XmlException e)
                {

                    App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                    System.Windows.MessageBox.Show(e.Message, "XmlException");
                    cleanup();
                    return null;
                }
                catch (XmlSchemaException e)
                {
                    App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                    System.Windows.MessageBox.Show(e.Message + "\nZeile " + e.LineNumber + ", Position " + e.LinePosition, "XmlSchemaException");
                    cleanup();
                    return null;
                }
                catch (FileNotFoundException e)
                {
                    App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                    System.Windows.MessageBox.Show(e.Message, "File not Found");
                    cleanup();
                    return null;
                }

                var NodeIterator = doc.CreateNavigator().Select("rezepte/rezept");

                while (NodeIterator.MoveNext())
                {
                    //var name = XmlHandler.NormalizeStr(navIT.Current.SelectSingleNode("name").Value);
                    var name = NormalizeStr(NodeIterator.Current.GetAttribute("name", ""));
                    var gruppe = NormalizeStr(NodeIterator.Current.SelectSingleNode("gruppe").Value);
                    var labor = NodeIterator.Current.SelectSingleNode("labor").Value;
                    var probe = (
                        NodeIterator.Current.SelectSingleNode("probe").SelectSingleNode("brauen").ValueAsInt,
                        NodeIterator.Current.SelectSingleNode("probe").SelectSingleNode("analyse").ValueAsInt
                        );
                    var rezept = new common.Rezept(name, gruppe, labor, probe)
                    {
                        Preis = NormalizeStr(NodeIterator.Current.SelectSingleNode("preis")?.Value),
                        Haltbarkeit = NormalizeStr(NodeIterator.Current.SelectSingleNode("haltbarkeit")?.Value),
                        Verbreitung = NormalizeStr(NodeIterator.Current.SelectSingleNode("verbreitung")?.Value),
                        Rezeptur = NormalizeStr(NodeIterator.Current.SelectSingleNode("rezeptur")?.Value),
                        Merkmale = NormalizeStr(NodeIterator.Current.SelectSingleNode("merkmale")?.Value),
                        Beschreibung = NormalizeStr(NodeIterator.Current.SelectSingleNode("beschreibung")?.Value),
                        Meisterhinweise = NormalizeStr(NodeIterator.Current.SelectSingleNode("meisterhinweise")?.Value)
                    };
                    
                    var currentNode = NodeIterator.Current.SelectSingleNode("beschaffung");
                    rezept.Beschaffung = (currentNode != null) ?
                        Tuple.Create(XmlHandler.NormalizeStr(currentNode.SelectSingleNode("kosten").Value), currentNode.SelectSingleNode("seltenheit").ValueAsInt) :
                        Tuple.Create("0", 0);
                    
                    currentNode = NodeIterator.Current.SelectSingleNode("seite");
                    rezept.Seite = (currentNode != null) ? currentNode.ValueAsInt : -1;

                    currentNode = NodeIterator.Current.SelectSingleNode("wirkung");
                    if (currentNode != null)
                    {
                        char[] arr = { 'M', 'A', 'B', 'C', 'D', 'E', 'F' };
                        foreach (char c in arr)
                        {
                            rezept.Wirkung[c] = (currentNode != null) ? XmlHandler.NormalizeStr(currentNode.SelectSingleNode(c.ToString()).Value) : null;
                        }
                    }

                    rezepte.Add(rezept);
                }
                cleanup();
                return rezepte;
            }
        }
        static public List<common.Rezept> ImportRezepteXml(string xmlLocation, string xsdLocation = null)
        {
            try
            {
                if(xsdLocation == null)
                {
                    return ImportRezepteXml(File.OpenRead(xmlLocation));
                }
                else
                {
                    return ImportRezepteXml(File.OpenRead(xmlLocation), File.OpenRead(xsdLocation));
                }
                
            }
              catch (FileNotFoundException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                System.Windows.MessageBox.Show(e.Message, "File not Found");
                return null;
            }
        }

        static public void ExportRezepteToXml(List<common.Rezept> rezepte, string filepath)
        {

            XmlDocument document = new XmlDocument();
            //TextWriter writer = new StreamWriter(filepath);
            var root = document.AppendChild(document.CreateElement("rezepte"));
            void createChild(XmlElement node, string name, string value)
            {
                node.AppendChild(document.CreateElement(name)).InnerText = NormalizeStr(value);
            }
            foreach (var rezept in rezepte)
            {
                var node = (XmlElement)root.AppendChild(document.CreateElement("rezept"));
                node.SetAttribute("name", NormalizeStr(NormalizeStr(rezept.Name)));
                createChild(node, "gruppe", rezept.Gruppe);
                var innerNode = (XmlElement)node.AppendChild(document.CreateElement("beschaffung"));
                createChild(innerNode, "kosten", rezept.Beschaffung.Item1);
                createChild(innerNode, "seltenheit", rezept.Beschaffung.Item2.ToString());
                createChild(node, "rezeptur", rezept.Rezeptur);
                createChild(node, "beschreibung", rezept.Beschreibung);
                createChild(node, "labor", rezept.Labor.Item2);
                innerNode = (XmlElement)node.AppendChild(document.CreateElement("probe"));
                createChild(innerNode, "brauen", rezept.Probe.Item1.ToString());
                createChild(innerNode, "analyse", rezept.Probe.Item2.ToString());
                innerNode = (XmlElement)node.AppendChild(document.CreateElement("wirkung"));
                char[] arr = { 'M', 'A', 'B', 'C', 'D', 'E', 'F' };
                foreach(char c in arr)
                {
                    createChild(innerNode, c.ToString(), rezept.Wirkung[c]);
                }
                createChild(node, "verbreitung", rezept.Verbreitung);
                createChild(node, "merkmale", rezept.Merkmale);
                createChild(node, "haltbarkeit", rezept.Haltbarkeit);
                createChild(node, "preis", rezept.Preis);
                createChild(node, "seite", rezept.Seite.ToString());
                createChild(node, "meisterhinweise", rezept.Meisterhinweise);
            }
            document.Normalize();
            document.Save(filepath);
        }

        static public void ResaveXml(string filename)
        {
            XmlDocument doc;
            try
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add("", $"data/{filename}.xsd");
                readerSettings.ValidationType = ValidationType.Schema;
                //schemaSet.Compile();
                readerSettings.Schemas = schemaSet;
                XmlReader reader = XmlReader.Create($"data/{filename}.xml", readerSettings);
                //XmlDocument doc = new XmlDocument();
                doc = new XmlDocument();
                doc.Load(reader);
            }
            catch (XmlException e)
            {
                System.Windows.MessageBox.Show(e.Message, "XmlException");
                return;
            }
            catch (XmlSchemaException e)
            {
                System.Windows.MessageBox.Show(e.Message + "\nZeile " + e.LineNumber + ", Position " + e.LinePosition, "XmlSchemaException");
                return;
            }
            var nav = doc.SelectNodes("Alchemie/rezept");
            foreach (XmlNode navIT in nav)
            {
                var tmp = navIT["beschaffung"];
                var t = tmp["kosten"].Value;
                if (tmp != null) { tmp["kosten"].InnerText = XmlHandler.NormalizeStr(tmp["kosten"].InnerText); tmp["seltenheit"].InnerText = XmlHandler.NormalizeStr(tmp["seltenheit"].InnerText); }
                tmp = navIT["wirkung"];
                if (tmp != null)
                {
                    char[] arr = { 'M', 'A', 'B', 'C', 'D', 'E', 'F' };
                    foreach (char c in arr)
                    {
                        tmp[c.ToString()].InnerText = XmlHandler.NormalizeStr(tmp[c.ToString()].InnerText);
                    }
                }
                string[] n = { "name", "gruppe", "labor", "verbreitung", "merkmale", "haltbarkeit", "rezeptur", "seite" };
                foreach (string s in n)
                {
                    tmp = navIT[s];
                    if (tmp != null) { tmp.InnerText = XmlHandler.NormalizeStr(tmp.InnerText); }

                }
            }
            doc.Save($"data/{filename}(copy).xml");
        }
    }
}
