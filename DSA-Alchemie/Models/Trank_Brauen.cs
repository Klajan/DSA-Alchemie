using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alchemie.Core;

namespace Alchemie.Models
{
    public partial class Trank : ObservableObject
    {
        public ExtendedObserableCollection<int> QualityDice { get; private set; } = new ExtendedObserableCollection<int>(new int[2] { 1, 1 });

        private int _TaPStarBrauen;

        public int TaPStarBrauen
        {
            get { return _TaPStarBrauen; }
            set { _TaPStarBrauen = value; RaisePropertyChange(); }
        }

        public char Brauen(int mod, (int rckHalten, int astralAuf, int misc) qualmod)
        {
            if (!_rezept.IsValid) return '?';
            if (_character == null) return '?';
            if (UseRNG) QualityDice.ReplaceRange(0, D6.Roll(2));
            int chym = _character.ChymischeHochzeit ? -1 : 0;
            int totalMod = mod + _rezept.Probe.BrauenMod + qualmod.rckHalten + Trank.CalculateLaborMod(_rezept.Labor.ID, _character.Labor) + chym + (int)_character.LaborQuality;
            TaPStarBrauen = TalentProbe(_character.TaWAutomatic, totalMod, _character.AttributesAutomatic);
            if (TaPStarBrauen < 0)
            {
                HaltbarkeitValue = -1;
                Quality = 'M';
                return Quality;
            }
            int qual = QualityDice[0] + QualityDice[1] + TaPStarBrauen + (qualmod.rckHalten * 2) + qualmod.astralAuf + qualmod.misc + (chym * -2);

            if (qual <= 6) { Quality = 'A'; }
            else if (qual <= 12) { Quality = 'B'; }
            else if (qual <= 18) { Quality = 'C'; }
            else if (qual <= 24) { Quality = 'D'; }
            else if (qual <= 30) { Quality = 'E'; }
            else { Quality = 'F'; }

            if (_rezept.Haltbarkeit.IsParsed())
            {
                HaltbarkeitValue = _rezept.Haltbarkeit.Dice.Roll();
            }

            return Quality;
        }
    }
}
