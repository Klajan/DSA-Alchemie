using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DSA_Alchemie
{
    public class Trank : Rezept
    {
        private readonly Rezept rezept;
        private readonly Random rnd;
        private char quality_;
        private string currentWirkung;
        public bool RNG { get; set; } = true;
        public char Quality
        {
            get { return quality_; }
            set
            {
                if ("MABCDEF".Contains(char.ToUpper(value)))
                {
                    quality_ = char.ToUpper(value);
                    currentWirkung = Wirkung[quality_];
                }
                else
                {
                    quality_ = '-';
                    currentWirkung = "";
                }
                RaisePropertyChange("Quality");
                RaisePropertyChange("CurrentWirkung");
            }
        }
        public DiceContainer RollEign { get; set; }
        public DiceContainer RollQual { get; set; }
        public string CurrentWirkung { get => currentWirkung; }

        public Trank() {}
        public Trank(Rezept rezept, Random rnd) : base(rezept)
        {
            this.rezept = rezept;
            this.rnd = rnd;
            RollEign = new DiceContainer("3W20", new Dice(1, 20, ref this.rnd), 3);
            RollQual = new DiceContainer("2W6", new Dice(1, 6, ref this.rnd), 2);
        }

        public bool IsSameBase(Rezept rezept)
        {
            return this.rezept.Equals(rezept);
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
            return TaW - mod - (Math.Max(RollEign.DiceList[0] - stats.Item1, 0) + Math.Max(RollEign.DiceList[1] - stats.Item2, 0) + Math.Max(RollEign.DiceList[2] - stats.Item3, 0));
        }
        public char Brauen(int mod, (int rckHalten, int astralAuf, int misc) qualmod, ref Character character)
        {
            if (!isValid) return '?';
            int chym = (character.schalenzauber.ChymischeHochzeit) ? -1 : 0;
            int totalMod = mod + base.Mods.Item1 + qualmod.rckHalten + Helper.CalcLaborMod(Labor.Item1, character.labor) + chym;
            int rest = Probe(character.alch, totalMod, (character.MU, character.KL, character.FF));
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
