﻿using System;

namespace Alchemie.Models
{
    public struct Labor
    {
        public Labor(string labor)
        {
            switch (labor)
            {
                case "0":
                case "archaisches Labor":
                    ID = LaborID.ArchaischesLabor;
                    Name = "archaisches Labor";
                    break;
                case "1":
                case "Hexenküche":
                    ID = LaborID.Hexenküche;
                    Name = "Hexenküche";
                    break;
                case "2":
                case "Alchimistenlabor":
                    ID = LaborID.Alchemielabor;
                    Name = "Alchimistenlabor";
                    break;
                default:
                    ID = LaborID.ArchaischesLabor;
                    Name = "Unbekannt";
                    break;
            }
        }
        public string Name { get; private set; }
        public LaborID ID { get; private set; }
    }

    public struct Probe
    {
        public Probe(int brauen, int analyse)
        {
            BrauenMod = brauen;
            AnalyseMod = analyse;
        }
        public int BrauenMod { get; private set; }
        public int AnalyseMod { get; private set; }
    }
    public struct Beschaffung
    {
        public Beschaffung(string preis, string verbreitung)
        {
            Preis = preis;
            Verbreitung = verbreitung;
        }
        public string Preis { get; private set; }
        public string Verbreitung { get; private set; }
    }

    public struct Wirkung
    {
        public Wirkung(string[] init)
        {
            M = A = B = C = D = E = F = String.Empty;

            if (init == null) return;

            for (int i = 0; i < 7 || i < init.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        M = init[i];
                        break;
                    case 1:
                        A = init[i];
                        break;
                    case 2:
                        B = init[i];
                        break;
                    case 3:
                        C = init[i];
                        break;
                    case 4:
                        D = init[i];
                        break;
                    case 5:
                        E = init[i];
                        break;
                    case 6:
                        F = init[i];
                        break;
                }
            }
        }
        public Wirkung(string m, string a, string b, string c, string d, string e, string f)
        {
            M = m; A = a; B = b; C = c; D = d; E = e; F = f;
        }
        public string M { get; private set; }
        public string A { get; private set; }
        public string B { get; private set; }
        public string C { get; private set; }
        public string D { get; private set; }
        public string E { get; private set; }
        public string F { get; private set; }

        public string this[char index]
        {

            get
            {
                switch (index)
                {
                    case 'M':
                        return M;
                    case 'A':
                        return A;
                    case 'B':
                        return B;
                    case 'C':
                        return C;
                    case 'D':
                        return D;
                    case 'E':
                        return E;
                    case 'F':
                        return F;
                    default:
                        return String.Empty;
                }
            }
        }
    }

    public class Rezept
    {
        private static uint lastID = 0;
        internal uint ID { private set; get; }
        internal bool IsValid { private set; get; }
        public string Name { private set; get; }
        public string Gruppe { private set; get; }
        public Labor Labor { private set; get; }
        public Probe Probe { private set; get; }
        public string Verbreitung { set; get; }
        public string Haltbarkeit { set; get; }
        public Beschaffung Beschaffung { set; get; }
        public string Preis { set; get; }
        public string Rezeptur { set; get; }
        public int Seite { set; get; }
        public string Merkmale { set; get; }
        public string Beschreibung { set; get; }
        public string Meisterhinweise { set; get; }
        public Wirkung Wirkung { set; get; }
        public Rezept() { IsValid = false; }
        public Rezept(string name, string group, string labor, (int brauen, int analyse) probe)
        {
            ID = lastID++;
            this.Name = name;
            this.Gruppe = group;
            this.Probe = new Probe(probe.brauen, probe.analyse);
            this.Labor = new Labor(labor);
            IsValid = true;
        }
        public Rezept(Rezept prevRezept)
        {
            if (prevRezept != null)
            {
                this.ID = prevRezept.ID;
                this.Name = prevRezept.Name;
                this.Gruppe = prevRezept.Gruppe;
                this.Labor = prevRezept.Labor;
                this.Probe = prevRezept.Probe;
                this.Verbreitung = prevRezept.Verbreitung;
                this.Haltbarkeit = prevRezept.Haltbarkeit;
                this.Beschaffung = prevRezept.Beschaffung;
                this.Preis = prevRezept.Preis;
                this.Wirkung = prevRezept.Wirkung;
                this.Merkmale = prevRezept.Merkmale;
                this.Seite = prevRezept.Seite;
                this.IsValid = prevRezept.IsValid;
            }
        }
    }
}
