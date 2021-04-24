using System;

namespace Alchemie.common
{
    public enum LabLvl : int
    {
        ArchaischesLabor = 0,
        Hexenküche = 1,
        Alchemielabor = 2,
    }

    public enum LabQual : int
    {
        Fehlend = 3,
        Normal = 0,
        Gut = -3,
        SehrGut = -7
    }

    public class Character
    {
        #region Members
        public int MU { set; get; } = 10;
        public int KL { set; get; } = 10;
        public int FF { set; get; } = 10;
        public int IN { set; get; } = 10;
        public int Alchemie { set; get; } = 5;
        public int Kochen { set; get; } = 5;
        public LabLvl Labor { set; get; } = LabLvl.ArchaischesLabor;
        public LabQual LaborQuality { set; get; } = LabQual.Normal;
        public bool AllegorischeAnalyse { set; get; } = false;
        public bool ChymischeHochzeit { set; get; } = false;
        public bool MandriconsBindung { set; get; } = false;
        public bool UsingAlchemie { set; get; } = true;

        public int AlchemieMH { set; get; } = 0;
        public int KochenMH { set; get; } = 0;
        #endregion Members
        public int TaW { set { }
            get
            {
                if (UsingAlchemie)
                {
                    return Alchemie + AlchemieMH;
                } else
                {
                    return Kochen + KochenMH;
                }
            }
        }
        #region Construction
        public Character()
        {
            
        }

        public Character(int mu1, int kl1, int ff1, int in1, int alchemie, int kochen, LabLvl lab = LabLvl.ArchaischesLabor, LabQual labqual = LabQual.Normal, bool usingAlch = true)
        {
            MU = mu1; KL = kl1; FF = ff1; IN = in1; Alchemie = alchemie; Kochen = kochen; Labor = lab; LaborQuality = labqual; UsingAlchemie = usingAlch;
        }
        #endregion Construction

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
        }
        #endregion Methods

        #region Factory
        public static Character LoadCharacterFromSettings()
        {
            return new Character(
                    Properties.CharacterSave.Default.MU,
                    Properties.CharacterSave.Default.KL,
                    Properties.CharacterSave.Default.FF,
                    Properties.CharacterSave.Default.IN,
                    Properties.CharacterSave.Default.Alchemie,
                    Properties.CharacterSave.Default.Kochen,
                    (LabLvl)Properties.CharacterSave.Default.LaborStufe,
                    (LabQual)Properties.CharacterSave.Default.LaborQuality,
                    Properties.CharacterSave.Default.UsingAlchemie
                );
        }
        #endregion Factory
    }
}
