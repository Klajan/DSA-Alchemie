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

        private string _expiryResultStr = string.Empty;

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
            TaPStarHaltbarkeit = TalentProbe(_character.TaWAlchemie, 9, _character.AttributesAlchemie, HaltbarkeitEigenschaftDice);
            if (_TaPStarHaltbarkeit >= 0)
            {
                ExpiryValue = _expiryBaseValue * 2;
                ExpiryResultStr = "doppelte Haltbarkeit";
            }
            else
            {
                int roll = UseRNG ? (ExpiryFailRoll = D6.Roll()) : ExpiryFailRoll;
                if (_TaPStarHaltbarkeit == -ushort.MaxValue) { roll += 4; }

                switch (roll)
                {
                    case <= 2:
                        ExpiryValue = (int)Math.Round(_expiryBaseValue * 2.0, MidpointRounding.AwayFromZero);
                        Quality = ChangeQualityBy(Quality, 0);
                        ExpiryResultStr = "0-2: doppelte Haltbarkeit";
                        break;
                    case 3:
                        ExpiryValue = (int)Math.Round(_expiryBaseValue * 1.5, MidpointRounding.AwayFromZero);
                        Quality = ChangeQualityBy(Quality, 0);
                        ExpiryResultStr = "3: anderthalbfache Haltbarkeit";
                        break;
                    case 4:
                        ExpiryValue = (int)Math.Round(_expiryBaseValue * 1.5, MidpointRounding.AwayFromZero);
                        Quality = ChangeQualityBy(Quality, -1);
                        ExpiryResultStr = "4: anderthalbfache Haltbarkeit, Qualität sinkt um 1 Stufe";
                        break;
                    case 5:
                        ExpiryValue = _expiryBaseValue;
                        Quality = ChangeQualityBy(Quality, -1);
                        ExpiryResultStr = "5: keine Veränderung der Haltbarkeit, Qualität sinkt um 1 Stufe";
                        break;
                    case <= 8:
                        ExpiryValue = (int)Math.Round(_expiryBaseValue * -1.0, MidpointRounding.AwayFromZero);
                        Quality = ChangeQualityBy(Quality, -9);
                        ExpiryResultStr = "6-8: Trank wird vollkommen wirkungslos";
                        break;
                    default:
                        ExpiryValue = _expiryBaseValue;
                        Quality = ChangeQualityBy(Quality, -9);
                        ExpiryResultStr = "9-10: die Wirkung des Trankes schlägt um in ein Gift (siehe Mandragora, GA 213: Stufe 2, 1W6 SP, Brechreiz/1W3 SP, +3 auf Handlungen)";
                        break;
                }
            }
            ExpiryIsExtended = true;
        }

        private void ResetHaltbarkeitToDefault(bool raiseEvent = true)
        {
            _TaPStarHaltbarkeit = 0;
            _expiryValue = -1;
            _expiryResultStr = string.Empty;
            ExpiryIsExtended = false;
            if (UseRNG)
            {
                ResetCollection(HaltbarkeitEigenschaftDice);
            }
            if (raiseEvent) RaisePropertyChange(null);
        }
    }
}