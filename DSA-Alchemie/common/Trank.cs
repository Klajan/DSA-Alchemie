using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Alchemie.common
{
    public class Trank : Rezept, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChange(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        private static readonly Dice D20 = new Dice(1, 20);
        private static readonly Dice D6 = new Dice(1, 6);
        #region Construction
        public Trank()
        {
            EigenschaftDice = new ExtendedObserableCollection<int>(new int[3] { 1, 1, 1 });
            QualityDice = new ExtendedObserableCollection<int>(new int[2] { 1, 1 });
        }
        public Trank(Rezept rezept) : base(rezept)
        {
            EigenschaftDice = new ExtendedObserableCollection<int>(new int[3] { 1, 1, 1 });
            QualityDice = new ExtendedObserableCollection<int>(new int[2] { 1, 1 });
        }
        public Trank(Rezept rezept, List<int> rollEign, List<int> rollQual) : base(rezept)
        {
            EigenschaftDice = new ExtendedObserableCollection<int>(rollEign);
            QualityDice = new ExtendedObserableCollection<int>(rollQual);
        }
        #endregion

        private char quality_;
        private string currentWirkung;
        private string currentMerkmale;
        public bool RNG { get; set; } = true;
        public char Quality
        {
            get { return quality_; }
            set
            {
                if ("MABCDEF".Contains(char.ToUpper(value, CultureInfo.CurrentCulture)))
                {
                    quality_ = char.ToUpper(value, CultureInfo.CurrentCulture);
                    currentWirkung = Wirkung[quality_];
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
        public ExtendedObserableCollection<int> EigenschaftDice { get; private set; }
        public ExtendedObserableCollection<int> QualityDice { get; private set; }
        public string CurrentWirkung { get => currentWirkung; }
        public string CurrentMerkmale { get => currentMerkmale; }

        

        public bool IsSameBase(Rezept rezept)
        {
            if (rezept != null)
            {
                return ID == rezept.ID;
            }
            return false;
        }

        private int TalentProbe(int TaW, int mod, (int, int, int) stats)
        {
            int c1 = 0, c20 = 0;
            foreach (int num in EigenschaftDice)
            {
                if (num >= 20) { c20++; }
                else if (num <= 1) { c1++; }
            }
            if (c1 >= 2) { return TaW; }
            if (c20 >= 2) { return int.MinValue; }
            if(TaW - mod >= 0)
            {
                return Math.Min(TaW,
                TaW - mod
                - (Math.Max(EigenschaftDice[0] - stats.Item1, 0)
                + Math.Max(EigenschaftDice[1] - stats.Item2, 0)
                + Math.Max(EigenschaftDice[2] - stats.Item3, 0))
                );
            } else
            {
                return Math.Min(TaW,
                0
                - (Math.Max(EigenschaftDice[0] - stats.Item1 + (mod - TaW), 0)
                + Math.Max(EigenschaftDice[1] - stats.Item2 + (mod - TaW), 0)
                + Math.Max(EigenschaftDice[2] - stats.Item3 + (mod - TaW), 0))
                );
            }
            
        }
        public char Brauen(int mod, (int rckHalten, int astralAuf, int misc) qualmod, Character character)
        {
            if (!isValid) return '?';
            if (character == null) return '?';
            if (RNG)
            {
                EigenschaftDice.ReplaceRange(0, D20.Roll(3));
                QualityDice.ReplaceRange(0, D6.Roll(2));
            }
            int chym = character.ChymischeHochzeit ? -1 : 0;
            int totalMod = mod + base.Probe.BrauenMod + qualmod.rckHalten + Helper.CalcLaborMod(Labor.ID, (int)character.Labor) + chym;
            int rest = TalentProbe(character.TaW, totalMod, character.UsingAlchemie ? (character.MU, character.KL, character.FF) : (character.KL, character.IN, character.FF));
            if (rest < 0)
            {
                Quality = 'M';
                return Quality;
            }
            int qual = QualityDice[0] + QualityDice[1] + rest + (qualmod.rckHalten * 2) + qualmod.astralAuf + qualmod.misc + (chym * -2);

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
