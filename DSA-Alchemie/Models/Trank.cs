using Alchemie.Core;
using System;
using System.Globalization;
using Alchemie.Models.Types;

namespace Alchemie.Models
{
    public partial class Trank : ObservableObject
    {
        private static readonly Dice D20 = new(1, 20);
        private static readonly Dice D6 = new(1, 6);

        #region Construction

        public Trank()
        {
        }

        public Trank(Rezept rezept)
        {
            _rezept = rezept;
        }

        public Trank(Character character)
        {
            _character = character;
        }

        public Trank(Rezept rezept, Character character) : this(rezept)
        {
            _character = character;
        }

        public Trank(Trank other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            _rezept = other._rezept;
            _character = other._character;
            _quality = other._quality;
            UseRNG = other.UseRNG;
            BrauenEigenschaftDice = new ExtendedObserableCollection<int>(other.BrauenEigenschaftDice);
            BrauenQualityDice = new ExtendedObserableCollection<int>(other.BrauenQualityDice);
        }

        #endregion Construction

        public bool UseRNG { get; set; } = true;
       
        private Rezept _rezept = new();

        public Rezept Rezept
        {
            get => _rezept;
            set
            {
                _rezept = value;
                ResetToDefault();
                RaisePropertyChange();
            }
        }

        private Character _character = new();

        public Character Character
        {
            get => _character;
            set
            {
                _character = value;
                RaisePropertyChange();
            }
        }

        private Quality _quality = Quality.None;

        public Quality Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                RaisePropertyChange();
                RaisePropertyChange(nameof(CurrentWirkung));
                //RaisePropertyChange(nameof(CurrentMerkmale));
            }
        }

        public string CurrentWirkung { get => Rezept.Wirkung[_quality]; }
        //public string CurrentMerkmale { get => Rezept.Merkmale; }

        private int TalentProbe(int TaW, int mod, (int, int, int) stats)
        {
            if (UseRNG) BrauenEigenschaftDice.ReplaceRange(0, D20.Roll(3));
            int c1 = 0, c20 = 0;
            foreach (int num in BrauenEigenschaftDice)
            {
                if (num >= 20) { c20++; }
                else if (num <= 1) { c1++; }
            }
            if (c1 >= 2) { return Math.Max(TaW - mod, 0); }
            if (c20 >= 2) { return -UInt16.MaxValue; }
            if (TaW - mod >= 0)
            {
                return Math.Min(TaW,
                TaW - mod
                - (Math.Max(BrauenEigenschaftDice[0] - stats.Item1, 0)
                + Math.Max(BrauenEigenschaftDice[1] - stats.Item2, 0)
                + Math.Max(BrauenEigenschaftDice[2] - stats.Item3, 0))
                );
            }
            else
            {
                return Math.Min(TaW,
                0
                - (Math.Max(BrauenEigenschaftDice[0] - stats.Item1 + (mod - TaW), 0)
                + Math.Max(BrauenEigenschaftDice[1] - stats.Item2 + (mod - TaW), 0)
                + Math.Max(BrauenEigenschaftDice[2] - stats.Item3 + (mod - TaW), 0))
                );
            }
        }

        private void ResetToDefault()
        {
            ResetBrauen();
            ResetHaltbarkeit();
            Quality = Quality.None;
        }

        public static Quality ChangeQualityBy(Quality quality, int change)
        {
            if (quality == Quality.None) return Quality.None;
            return (Quality)Math.Clamp((int)quality + change, 0, 6);
        }

        public static int CalculateLaborMod(LaborID RezeptLabor, LaborID CharLabor)
        {
            return (RezeptLabor - CharLabor) switch
            {
                -2 => -3,
                -1 => 0,
                0 => 0,
                +1 => +7,
                _ => UInt16.MaxValue,
            };
        }
    }
}