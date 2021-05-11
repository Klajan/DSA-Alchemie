using Alchemie.Core;
using Alchemie.Models.Types;

namespace Alchemie.Models
{
    public class Character : ObservableObject
    {
        #region Members

        public int MU { set; get; } = 10;
        public int KL { set; get; } = 10;
        public int FF { set; get; } = 10;
        public int IN { set; get; } = 10;
        public int Alchemie { set; get; } = 5;
        public int Kochen { set; get; } = 5;

        private LaborID _labor = LaborID.ArchaischesLabor;
        private LaborQL _laborQuality = LaborQL.Normal;
        private bool _chymischeHochzeit;

        public bool AllegorischeAnalyse { set; get; }
        public bool MandriconsBindung { set; get; }
        public bool UsingAlchemie { set; get; } = true;

        public int AlchemieMH { set; get; }
        public int KochenMH { set; get; }

        #endregion Members

        #region Properties

        public LaborQL LaborQuality
        {
            get { return _laborQuality; }
            set
            { _laborQuality = value; RaisePropertyChange(); }
        }

        public LaborID Labor
        {
            get { return _labor; }
            set { _labor = value; RaisePropertyChange(); }
        }

        public bool ChymischeHochzeit
        {
            get { return _chymischeHochzeit; }
            set { _chymischeHochzeit = value; RaisePropertyChange(); }
        }

        #endregion Properties

        public int TaWAlchemie => Alchemie + AlchemieMH;

        public int TaWKochen => Kochen + KochenMH;

        public int TaWAutomatic => UsingAlchemie ? TaWAlchemie : TaWKochen;

        public (int, int, int) AttributesAlchemie => (MU, KL, FF);

        public (int, int, int) AttributesKochen => (KL, IN, FF);

        public (int, int, int) AttributesAutomatic => UsingAlchemie ? AttributesAlchemie : AttributesKochen;

        #region Methods

        public void SaveCharacterToSettings()
        {
            Properties.CharacterSave.Default.MU = MU;
            Properties.CharacterSave.Default.KL = KL;
            Properties.CharacterSave.Default.FF = FF;
            Properties.CharacterSave.Default.IN = IN;
            Properties.CharacterSave.Default.Alchemie = Alchemie;
            Properties.CharacterSave.Default.Kochen = Kochen;
            Properties.CharacterSave.Default.UsingAlchemie = UsingAlchemie;
            Properties.CharacterSave.Default.LaborStufe = (int)Labor;
            Properties.CharacterSave.Default.LaborQuality = (int)LaborQuality;
            Properties.CharacterSave.Default.AllegorischeAnalyse = AllegorischeAnalyse;
            Properties.CharacterSave.Default.ChymischeHochzeit = ChymischeHochzeit;
            Properties.CharacterSave.Default.MandriconsBindung = MandriconsBindung;
        }

        #endregion Methods

        #region Factory

        public static Character LoadCharacterFromSettings()
        {
            return new Character()
            {
                MU = Properties.CharacterSave.Default.MU,
                KL = Properties.CharacterSave.Default.KL,
                FF = Properties.CharacterSave.Default.FF,
                IN = Properties.CharacterSave.Default.IN,
                Alchemie = Properties.CharacterSave.Default.Alchemie,
                Kochen = Properties.CharacterSave.Default.Kochen,
                Labor = (LaborID)Properties.CharacterSave.Default.LaborStufe,
                LaborQuality = (LaborQL)Properties.CharacterSave.Default.LaborQuality,
                UsingAlchemie = Properties.CharacterSave.Default.UsingAlchemie,
                AllegorischeAnalyse = Properties.CharacterSave.Default.AllegorischeAnalyse,
                ChymischeHochzeit = Properties.CharacterSave.Default.ChymischeHochzeit,
                MandriconsBindung = Properties.CharacterSave.Default.MandriconsBindung
            };
        }

        #endregion Factory
    }
}