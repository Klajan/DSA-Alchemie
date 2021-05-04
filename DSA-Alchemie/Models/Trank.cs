using Alchemie.Core;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Alchemie.Models
{
    public class Trank : ObservableObject
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

        public Trank(Rezept rezept, IList<int> rollEign, IList<int> rollQual) : this(rezept)
        {
            EigenschaftDice = new ExtendedObserableCollection<int>(rollEign);
            QualityDice = new ExtendedObserableCollection<int>(rollQual);
        }

        public Trank(Rezept rezept, Character character, IList<int> rollEign, IList<int> rollQual) : this(rezept, rollEign, rollQual)
        {
            _character = character;
        }

        #endregion Construction

        private Rezept _rezept = new();

        public Rezept Rezept
        {
            get => _rezept;
            set
            {
                _rezept = value;
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

        private char quality_;
        private string currentWirkung;
        private string currentMerkmale;
        public bool UseRNG { get; set; } = true;

        public char Quality
        {
            get { return quality_; }
            set
            {
                if ("MABCDEF".Contains(char.ToUpper(value, CultureInfo.CurrentCulture), StringComparison.CurrentCultureIgnoreCase))
                {
                    quality_ = char.ToUpper(value, CultureInfo.CurrentCulture);
                    currentWirkung = _rezept.Wirkung[quality_];
                    currentMerkmale = _rezept.Merkmale;
                }
                else
                {
                    quality_ = '?';
                    currentWirkung = String.Empty;
                    currentMerkmale = String.Empty;
                }
                RaisePropertyChange("Quality");
                RaisePropertyChange("CurrentWirkung");
                RaisePropertyChange("CurrentMerkmale");
            }
        }

        public ExtendedObserableCollection<int> EigenschaftDice { get; private set; } = new ExtendedObserableCollection<int>(new int[3] { 1, 1, 1 });
        public ExtendedObserableCollection<int> QualityDice { get; private set; } = new ExtendedObserableCollection<int>(new int[2] { 1, 1 });
        public string CurrentWirkung { get => currentWirkung; }
        public string CurrentMerkmale { get => currentMerkmale; }

        private int TalentProbe(int TaW, int mod, (int, int, int) stats)
        {
            int c1 = 0, c20 = 0;
            foreach (int num in EigenschaftDice)
            {
                if (num >= 20) { c20++; }
                else if (num <= 1) { c1++; }
            }
            if (c1 >= 2) { return TaW; }
            if (c20 >= 2) { return Int16.MinValue; }
            if (TaW - mod >= 0)
            {
                return Math.Min(TaW,
                TaW - mod
                - (Math.Max(EigenschaftDice[0] - stats.Item1, 0)
                + Math.Max(EigenschaftDice[1] - stats.Item2, 0)
                + Math.Max(EigenschaftDice[2] - stats.Item3, 0))
                );
            }
            else
            {
                return Math.Min(TaW,
                0
                - (Math.Max(EigenschaftDice[0] - stats.Item1 + (mod - TaW), 0)
                + Math.Max(EigenschaftDice[1] - stats.Item2 + (mod - TaW), 0)
                + Math.Max(EigenschaftDice[2] - stats.Item3 + (mod - TaW), 0))
                );
            }
        }

        public char Brauen(int mod, (int rckHalten, int astralAuf, int misc) qualmod)
        {
            if (!_rezept.IsValid) return '?';
            if (_character == null) return '?';
            if (UseRNG)
            {
                EigenschaftDice.ReplaceRange(0, D20.Roll(3));
                QualityDice.ReplaceRange(0, D6.Roll(2));
            }
            int chym = _character.ChymischeHochzeit ? -1 : 0;
            int totalMod = mod + _rezept.Probe.BrauenMod + qualmod.rckHalten + Trank.CalculateLaborMod(_rezept.Labor.ID, _character.Labor) + chym + (int)_character.LaborQuality;
            int rest = TalentProbe(_character.TaW, totalMod, _character.UsingAlchemie ? (_character.MU, _character.KL, _character.FF) : (_character.KL, _character.IN, _character.FF));
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

        public static int CalculateLaborMod(LaborID RezeptLabor, LaborID CharLabor)
        {
            return (RezeptLabor - CharLabor) switch
            {
                -2 => -3,
                -1 => 0,
                0 => 0,
                +1 => +7,
                _ => Int16.MaxValue,
            };
        }
    }
}