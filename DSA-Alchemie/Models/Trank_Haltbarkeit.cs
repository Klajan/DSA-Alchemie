using Alchemie.Core;
using System;

namespace Alchemie.Models
{
    public partial class Trank : ObservableObject
    {
        public ExtendedObserableCollection<int> HaltbarkeitEigenschaftDice { get; private set; } = new ExtendedObserableCollection<int>(new int[3] { 1, 1, 1 });

        private int _expiryFailRoll = 1;

        public int ExpiryFailRoll
        {
            get => _expiryFailRoll;
            set => SetValue(ref _expiryFailRoll, value);
        }

        private int _expiryBaseValue = -1;

        public int ExpiryBaseValue
        {
            get => _expiryBaseValue;
            set => SetValue(ref _expiryBaseValue, value);
        }

        private int _expiryValue = -1;

        public int ExpiryValue
        {
            get => _expiryValue;
            set => SetValue(ref _expiryValue, value);
        }

        private string _expiryResultStr = String.Empty;

        public string ExpiryResultStr
        {
            get => _expiryResultStr;
            protected set => SetValue(ref _expiryResultStr, value);
        }

        private int _TaPStarHaltbarkeit;

        public int TaPStarHaltbarkeit
        {
            get => _TaPStarHaltbarkeit;
            protected set => SetValue(ref _TaPStarHaltbarkeit, value);
        }

        public bool ExpiryIsExtended { get; set; }

        public void HaltbarkeitVerlängern()
        {
            const string S1 = "doppelte Haltbarkeit";
            const string S2 = "3: anderthalbfache Haltbarkeit";
            const string S3 = "4: anderthalbfache Haltbarkeit, Qualität sinkt um 1 Stufe";
            const string S4 = "5: keine Veränderung der Haltbarkeit, Qualität sinkt um 1 Stufe";
            const string S5 = "6-8: Trank wird vollkommen wirkungslos";
            const string S6 = "9-10: die Wirkung des Trankes schlägt um in ein Gift (siehe Mandragora, GA 213: Stufe 2, 1W6 SP, Brechreiz/1W3 SP, +3 auf Handlungen)";

            TaPStarHaltbarkeit = TalentProbe(_character.TaWAlchemie, 9, _character.AttributesAlchemie, HaltbarkeitEigenschaftDice);
            if (_TaPStarHaltbarkeit >= 0)
            {
                ExpiryValue = _expiryBaseValue * 2;
                ExpiryResultStr = S1;
            }
            else
            {
                int roll;
                if (UseRNG) { roll = ExpiryFailRoll = D6.Roll(); }
                else { roll = ExpiryFailRoll; }
                if (_TaPStarHaltbarkeit == -UInt16.MaxValue) roll += 4;
                (double, int, string) result = new Func<(double, int, string)>(() =>
                    {
                        if (roll <= 2) return (2.0, 0, String.Concat("0-2: ", S1));
                        if (roll <= 3) return (1.5, 0, S2);
                        if (roll <= 4) return (1.5, -1, S3);
                        if (roll <= 5) return (1.0, -1, S4);
                        if (roll <= 8) return (-1.0, -9, S5);
                        return (0.0, -9, S6);
                    })();
                ExpiryValue = (int)Math.Round((double)_expiryBaseValue * result.Item1, MidpointRounding.AwayFromZero);
                Quality = ChangeQualityBy(Quality, result.Item2);
                ExpiryResultStr = result.Item3;
            }
            ExpiryIsExtended = true;
        }

        private void ResetHaltbarkeitToDefault(bool raiseEvent = true)
        {
            _TaPStarHaltbarkeit = 0;
            _expiryValue = -1;
            _expiryResultStr = String.Empty;
            ExpiryIsExtended = false;
            if (UseRNG)
            {
                ResetCollection(HaltbarkeitEigenschaftDice);
            }
            if (raiseEvent) RaisePropertyChange(null);
        }
    }
}