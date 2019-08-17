using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DSA_Alchemie.dataClasses
{
    public class Trank : Rezept
    {
        private char quality_;
        private string currentWirkung;
        private string currentMerkmale;
        public bool RNG { get; set; } = true;
        public char Quality
        {
            get { return quality_; }
            set
            {
                if ("MABCDEF".Contains(char.ToUpper(value)))
                {
                    quality_ = char.ToUpper(value);
                    currentWirkung = Wirkung.ContainsKey(value) ? Wirkung[quality_] : String.Empty;
                    currentMerkmale = Merkmale;
                }
                else
                {
                    quality_ = '-';
                    currentWirkung = String.Empty;
                    currentMerkmale = String.Empty;
                }
                RaisePropertyChange("Quality");
                RaisePropertyChange("CurrentWirkung");
                RaisePropertyChange("CurrentMerkmale");
            }
        }
        public DiceContainer RollEign { get; set; }
        public DiceContainer RollQual { get; set; }
        public string CurrentWirkung { get => currentWirkung; }
        public string CurrentMerkmale { get => currentMerkmale; }

        public Trank() {}
        public Trank(Rezept rezept) : base(rezept)
        {
            RollEign = new DiceContainer("3W20", new Dice(1, 20), 3);
            RollQual = new DiceContainer("2W6", new Dice(1, 6), 2);
        }
        public Trank(Rezept rezept, DiceContainer rollEign, DiceContainer rollQual) : base(rezept)
        {
            RollEign = new DiceContainer(rollEign);
            RollQual = new DiceContainer(rollQual);
        }

        public bool IsSameBase(Rezept rezept)
        {
            return ID == rezept.ID;
        }

        private int Probe(int TaW, int mod, (int, int, int) stats)
        {
            if (RNG) RollEign.Roll();
            int c1 = 0, c20 = 0;
            foreach (int num in RollEign.DiceList)
            {
                if (num == 20) { c20++; }
                else if (num == 1) { c1++; }
            }
            if (c1 >= 2) { return TaW; }
            if (c20 >= 2) { return -99; }
            return Math.Min(TaW, TaW - mod - (Math.Max(RollEign.DiceList[0] - stats.Item1, 0) + Math.Max(RollEign.DiceList[1] - stats.Item2, 0) + Math.Max(RollEign.DiceList[2] - stats.Item3, 0)));
        }
        public char Brauen(int mod, (int rckHalten, int astralAuf, int misc) qualmod, Character character)
        {
            if (!isValid) return '?';
            int chym = (character.ChymischeHochzeit) ? -1 : 0;
            int totalMod = mod + base.Mods.Item1 + qualmod.rckHalten + Helper.CalcLaborMod(Labor.Item1, character.Labor) + chym;
            int rest = Probe(character.Alchemie, totalMod, (character.MU, character.KL, character.FF));
            if (rest < 0)
            {
                Quality = 'M';
                return Quality;
            }
            if (RNG) RollQual.Roll();
            int qual = RollQual.DiceList[0] + RollQual.DiceList[1] + rest + (qualmod.rckHalten * 2) + qualmod.astralAuf + qualmod.misc + (chym * -2);
            if (qual <= 6) { Quality = 'A'; }
            else if (qual <= 12) { Quality = 'B'; }
            else if (qual <= 18) { Quality = 'C'; }
            else if (qual <= 24) { Quality = 'D'; }
            else if (qual <= 30) { Quality = 'E'; }
            else { Quality = 'F'; }
            return Quality;
        }
    }
}
