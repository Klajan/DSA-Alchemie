using Alchemie.Models;
using Alchemie.Models.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Alchemie
{
    public static class XmlHandler
    {
        static private string NormalizeStr(string input)
        {
            if (input is null) return null;
            return Regex.Replace(
                Regex.Replace(input, @"\r\n?|\n", string.Empty, RegexOptions.CultureInvariant),
                @"\s+", " ", RegexOptions.CultureInvariant
                ).Trim();
        }

        static private XmlSchema GetSchema(Stream xsdStream)
        {
            static void ValidationCallBack(object sender, ValidationEventArgs args)
            {
                return;
            }
            using XmlReader reader = XmlReader.Create(xsdStream);
            return XmlSchema.Read(reader, ValidationCallBack);
        }

        static public IList<Rezept> ImportRezepteXml(Stream xmlStream, Stream xsdStream = null)
        {
            static Beschaffung GetBeschaffung(XPathNavigator node)
            {
                if (node is null) return default;
                return new Beschaffung(NormalizeStr(node.SelectSingleNode("kosten")?.Value), NormalizeStr(node.SelectSingleNode("seltenheit")?.Value));
            }

            static int GetSeite(XPathNavigator node)
            {
                if (node is null) return 0;
                return node.ValueAsInt;
            }

            static Wirkung GetWirkung(XPathNavigator node)
            {
                if (node is null) return default;
                string[] init = new string[7];
                for (int i = 0; i < 7; i++)
                {
                    init[i] = NormalizeStr(node.SelectSingleNode(((Quality)i).ToString()).Value);
                }
                return new(init);
            }

            List<Rezept> rezepte = new();
            XmlSchemaSet schemaSet = new();
            XmlReaderSettings readerSettings = new();

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
                using XmlReader reader = XmlReader.Create(xmlStream, readerSettings);
                doc = new XPathDocument(reader, XmlSpace.Preserve);

                XPathNodeIterator NodeIterator = doc.CreateNavigator().Select("rezepte/rezept");
                while (NodeIterator.MoveNext())
                {
                    rezepte.Add(new(
                        NormalizeStr(NodeIterator.Current.GetAttribute("name", "")),
                        NormalizeStr(NodeIterator.Current.SelectSingleNode("gruppe").Value),
                        NormalizeStr(NodeIterator.Current.SelectSingleNode("labor").Value),
                        (NodeIterator.Current.SelectSingleNode("probe").SelectSingleNode("brauen").ValueAsInt,
                        NodeIterator.Current.SelectSingleNode("probe").SelectSingleNode("analyse").ValueAsInt))
                    {
                        Preis = NormalizeStr(NodeIterator.Current.SelectSingleNode("preis")?.Value),
                        Haltbarkeit = new(NormalizeStr(NodeIterator.Current.SelectSingleNode("haltbarkeit")?.Value)),
                        Verbreitung = NormalizeStr(NodeIterator.Current.SelectSingleNode("verbreitung")?.Value),
                        Rezeptur = NormalizeStr(NodeIterator.Current.SelectSingleNode("rezeptur")?.Value),
                        Merkmale = NormalizeStr(NodeIterator.Current.SelectSingleNode("merkmale")?.Value),
                        Beschreibung = NormalizeStr(NodeIterator.Current.SelectSingleNode("beschreibung")?.Value),
                        Meisterhinweise = NormalizeStr(NodeIterator.Current.SelectSingleNode("meisterhinweise")?.Value),
                        Beschaffung = GetBeschaffung(NodeIterator.Current.SelectSingleNode("beschaffung")),
                        Seite = GetSeite(NodeIterator.Current.SelectSingleNode("seite")),
                        Wirkung = GetWirkung(NodeIterator.Current.SelectSingleNode("wirkung"))
                    });
                }
            }
            catch (XmlException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                System.Windows.MessageBox.Show(e.Message, Properties.ErrorStrings.XmlException);
                return null;
            }
            catch (XmlSchemaException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                System.Windows.MessageBox.Show(e.Message + Properties.ErrorStrings.XsdExceptionMsg1 + e.LineNumber + Properties.ErrorStrings.XsdExceptionMsg2 + e.LinePosition, Properties.ErrorStrings.XsdException);
                return null;
            }
            catch (FileNotFoundException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                System.Windows.MessageBox.Show(e.Message, Properties.ErrorStrings.FileNotFoundException);
                return null;
            }
            return rezepte;
        }

        static public IList<Rezept> ImportRezepteXml(string xmlLocation, string xsdLocation = null)
        {
            try
            {
                using FileStream xmlstream = File.OpenRead(xmlLocation);
                if (xsdLocation == null)
                {
                    return ImportRezepteXml(xmlstream);
                }
                else
                {
                    using FileStream xsdstream = File.OpenRead(xsdLocation);
                    return ImportRezepteXml(xmlstream, xsdstream);
                }
            }
            catch (FileNotFoundException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                System.Windows.MessageBox.Show(e.Message, Properties.ErrorStrings.FileNotFoundException);
                return null;
            }
        }

#if DEBUG
#pragma warning disable

        static public void ExportRezepteToXml(List<Rezept> rezepte, string filepath)
        {
            XmlDocument document = new XmlDocument();

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
                createChild(innerNode, "kosten", rezept.Beschaffung.Preis);
                createChild(innerNode, "seltenheit", rezept.Beschaffung.Verbreitung);
                createChild(node, "rezeptur", rezept.Rezeptur);
                createChild(node, "beschreibung", rezept.Beschreibung);
                createChild(node, "labor", rezept.Labor.Name);
                innerNode = (XmlElement)node.AppendChild(document.CreateElement("probe"));
                createChild(innerNode, "brauen", rezept.Probe.BrauenMod.ToString(CultureInfo.CurrentCulture));
                createChild(innerNode, "analyse", rezept.Probe.AnalyseMod.ToString(CultureInfo.CurrentCulture));
                innerNode = (XmlElement)node.AppendChild(document.CreateElement("wirkung"));
                char[] arr = { 'M', 'A', 'B', 'C', 'D', 'E', 'F' };
                foreach (char c in arr)
                {
                    createChild(innerNode, c.ToString(CultureInfo.CurrentCulture), rezept.Wirkung[c]);
                }
                createChild(node, "verbreitung", rezept.Verbreitung);
                createChild(node, "merkmale", rezept.Merkmale);
                createChild(node, "haltbarkeit", rezept.Haltbarkeit.ToString());
                createChild(node, "preis", rezept.Preis);
                createChild(node, "seite", rezept.Seite.ToString(CultureInfo.CurrentCulture));
                createChild(node, "meisterhinweise", rezept.Meisterhinweise);
            }
            document.Normalize();
            document.Save(filepath);
        }

#pragma warning restore
#endif
    }
}