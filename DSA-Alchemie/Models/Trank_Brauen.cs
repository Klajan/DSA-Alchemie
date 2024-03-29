﻿using Alchemie.Core;
using Alchemie.Models.Types;

namespace Alchemie.Models
{
    public partial class Trank : ObservableObject
    {
        public ExtendedObserableCollection<int> BrauenEigenschaftDice { get; private set; } = new ExtendedObserableCollection<int>(new int[3] { 1, 1, 1 });
        public ExtendedObserableCollection<int> BrauenQualityDice { get; private set; } = new ExtendedObserableCollection<int>(new int[2] { 1, 1 });

        private int _TaPStarBrauen;

        public int TaPStarBrauen
        {
            get { return _TaPStarBrauen; }
            set { SetValue(ref _TaPStarBrauen, value); }
        }

        public Quality Brauen(int mod, (int rckHalten, int astralAuf, int misc) qualmod)
        {
            if (!_rezept.IsValid || _character == null) return Quality.None;
            if (UseRNG) BrauenQualityDice.ReplaceRange(0, D6.Roll(2));
            int chym = _character.ChymischeHochzeit ? -1 : 0;
            int totalMod = mod + _rezept.Probe.BrauenMod + qualmod.rckHalten + CalculateLaborMod(_rezept.Labor.ID, _character.Labor) + chym + (int)_character.LaborQuality;
            TaPStarBrauen = TalentProbe(_character.TaWAutomatic, totalMod, _character.AttributesAutomatic, BrauenEigenschaftDice);
            if (TaPStarBrauen < 0)
            {
                ExpiryBaseValue = -1;
                Quality = Quality.M;
                return Quality;
            }
            int qual = BrauenQualityDice[0] + BrauenQualityDice[1] + TaPStarBrauen + (qualmod.rckHalten * 2) + qualmod.astralAuf + qualmod.misc + (chym * -2);

            if (qual <= 6) { Quality = Quality.A; }
            else if (qual <= 12) { Quality = Quality.B; }
            else if (qual <= 18) { Quality = Quality.C; }
            else if (qual <= 24) { Quality = Quality.D; }
            else if (qual <= 30) { Quality = Quality.E; }
            else { Quality = Quality.F; }

            if (_rezept.Haltbarkeit.IsParsed)
            {
                ExpiryBaseValue = _rezept.Haltbarkeit.Dice.Roll();
            }

            return Quality;
        }

        private void ResetBrauenToDefault(bool raiseEvent = true)
        {
            _TaPStarBrauen = 0;
            _expiryBaseValue = -1;
            if (UseRNG)
            {
                ResetCollection(BrauenEigenschaftDice);
                ResetCollection(BrauenQualityDice);
            }
            if (raiseEvent) RaisePropertyChange(null);
        }
    }
}