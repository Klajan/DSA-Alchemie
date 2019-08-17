﻿using System;
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


namespace DSA_Alchemie
{
    public class XmlHandler
    {
        private static Regex xmlRegex;
        static public Mutex mutex = new Mutex();
        static private string normalizeS(string input)
        {
            return Regex.Replace(Regex.Replace(input, @"\r\n?|\n", ""), @"\s+", " ").Trim();
        }
        static private XmlSchema getSchema()
        {
            void ValidationCallBack(object sender, ValidationEventArgs args)
            {
                return;
            }
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DSA_Alchemie.data.data.xsd";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            return XmlSchema.Read(stream, ValidationCallBack);
        }
        static public void ImportXmlData(string filename, ref dataClasses.Database db)
        {
            mutex.WaitOne();
            XPathDocument doc;
            try
            {
                    //DSA_Alchemie.Database
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(getSchema());
                readerSettings.ValidationType = ValidationType.Schema;
                //schemaSet.Compile();
                readerSettings.Schemas = schemaSet;
                XmlReader reader = XmlReader.Create(filename, readerSettings);
                //XmlDocument doc = new XmlDocument();
                doc = new XPathDocument(reader);
            }
            catch(XmlException e)
            {
                
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                System.Windows.MessageBox.Show(e.Message, "XmlException");
                mutex.ReleaseMutex();
                return;
            }
            catch(XmlSchemaException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                System.Windows.MessageBox.Show(e.Message + "\nZeile " + e.LineNumber + ", Position " + e.LinePosition, "XmlSchemaException");
                mutex.ReleaseMutex();
                return;
            }
            catch(FileNotFoundException e)
            {
                App.Exceptions.Add(Tuple.Create(e as Exception, e.GetType()));
                System.Windows.MessageBox.Show(e.Message, "File not Found");
                mutex.ReleaseMutex();
                return;
            }
            var nav = doc.CreateNavigator();
            var navIT = nav.Select("Alchemie/rezept");
            while (navIT.MoveNext())
            { 
                var name = XmlHandler.normalizeS(navIT.Current.SelectSingleNode("name").Value);
                var gruppe = XmlHandler.normalizeS(navIT.Current.SelectSingleNode("gruppe").Value);
                var labor = navIT.Current.SelectSingleNode("labor").ValueAsInt;
                var tmp = navIT.Current.SelectSingleNode("probe");
                var probe = (tmp.SelectSingleNode("brauen").ValueAsInt, tmp.SelectSingleNode("analyse").ValueAsInt);
                var rezept = new dataClasses.Rezept(name, gruppe, labor, probe);
                tmp = navIT.Current.SelectSingleNode("beschaffung");
                rezept.Beschaffung = (tmp != null) ?
                    Tuple.Create(XmlHandler.normalizeS(tmp.SelectSingleNode("kosten").Value), tmp.SelectSingleNode("seltenheit").ValueAsInt) : 
                    Tuple.Create("0", 0);
                tmp = navIT.Current.SelectSingleNode("preis");
                rezept.Preis = (tmp != null) ? XmlHandler.normalizeS(tmp.Value) : null;
                tmp = navIT.Current.SelectSingleNode("haltbarkeit");
                rezept.Haltbarkeit = (tmp != null) ? XmlHandler.normalizeS(tmp.Value) : null;
                tmp = navIT.Current.SelectSingleNode("verbreitung");
                rezept.Verbreitung = (tmp != null) ? XmlHandler.normalizeS(tmp.Value) : null;
                tmp = navIT.Current.SelectSingleNode("seite");
                rezept.Seite = (tmp != null) ? tmp.ValueAsInt : -1;
                tmp = navIT.Current.SelectSingleNode("rezeptur");
                rezept.Zutaten = (tmp != null) ? XmlHandler.normalizeS(tmp.Value) : null;
                tmp = navIT.Current.SelectSingleNode("merkmale");
                rezept.Merkmale = (tmp != null) ? XmlHandler.normalizeS(tmp.Value) : null;
                tmp = navIT.Current.SelectSingleNode("wirkung");
                if (tmp != null)
                {
                    char[] arr = { 'M', 'A', 'B', 'C', 'D', 'E', 'F' };
                    foreach (char c in arr)
                    {
                        rezept.Wirkung[c] = (tmp != null) ? XmlHandler.normalizeS(tmp.SelectSingleNode(c.ToString()).Value) : null;
                    }
                }
                try
                {
                    db.AddRezept(rezept);
                }
                catch (ArgumentException e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Duplicate Key");
                    mutex.ReleaseMutex();
                    return;
                }
            }
            mutex.ReleaseMutex();
        }
        static public void reformatXml(string filename)
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
            foreach(XmlNode navIT in nav)
            {
                var tmp = navIT["beschaffung"];
                var t = tmp["kosten"].Value;
                if (tmp != null) { tmp["kosten"].InnerText = XmlHandler.normalizeS(tmp["kosten"].InnerText); tmp["seltenheit"].InnerText = XmlHandler.normalizeS(tmp["seltenheit"].InnerText); }
                tmp = navIT["wirkung"];
                if (tmp != null)
                {
                    char[] arr = { 'M', 'A', 'B', 'C', 'D', 'E', 'F' };
                    foreach (char c in arr)
                    {
                        tmp[c.ToString()].InnerText = XmlHandler.normalizeS(tmp[c.ToString()].InnerText);
                    }
                }
                string[] n = { "name", "gruppe", "labor", "verbreitung", "merkmale", "haltbarkeit", "rezeptur", "seite" };
                foreach (string s in n)
                {
                    tmp = navIT[s];
                    if(tmp != null) { tmp.InnerText = XmlHandler.normalizeS(tmp.InnerText); }
                   
                }
            }
            doc.Save($"data/{filename}(copy).xml");
        }
    }
}
