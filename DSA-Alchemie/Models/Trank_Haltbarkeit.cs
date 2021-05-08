using Alchemie.Core;
using System;

namespace Alchemie.Models
{
    public partial class Trank : ObservableObject
    {
        private int _expiryValue = -1;

        public int HaltbarkeitValue
        {
            get { return _expiryValue; }
            set { _expiryValue = value; RaisePropertyChange(); RaisePropertyChange(nameof(HaltbarkeitString)); }
        }

        public string HaltbarkeitString { get => _rezept.Haltbarkeit.GetValueString(_expiryValue); }

        public string HaltbarkeitFailedString;

        public void HaltbarkeitVerlängern()
        {
            const string S1 = "doppelte Haltbarkeit";
            const string S2 = "anderthalbfache Haltbarkeit";
            const string S3 = "anderthalbfache Haltbarkeit, Qualität sinkt um 1 Stufe";
            const string S4 = "keine Veränderung der Haltbarkeit, Qualität sinkt um 1 Stufe";
            const string S5 = "Trank wird vollkommen wirkungslos";
            const string S6 = "die Wirkung des Trankes schlägt um in ein Gift (siehe Mandragora, GA 213: Stufe 2, 1W6 SP, Brechreiz/1W3 SP, +3 auf Handlungen)";

            int rest = TalentProbe(_character.AlchemieMH, 9, _character.AttributesAlchemie);
            if (rest >= 0)
            {
                HaltbarkeitValue *= 2;
            }
            else
            {
                int roll = D6.Roll();
                if (rest == -UInt16.MinValue) roll += 4;
                (double, int, string) result = new Func<(double, int, string)>(() =>
                    {
                        if (roll <= 2) return (2.0, 0, S1);
                        if (roll <= 3) return (1.5, 0, S2);
                        if (roll <= 4) return (1.5, -1, S3);
                        if (roll <= 5) return (1.0, -1, S4);
                        if (roll <= 8) return (0.0, -9, S5);
                        return (0.0, -9, S6);
                    })();
            }
        }
    }
}