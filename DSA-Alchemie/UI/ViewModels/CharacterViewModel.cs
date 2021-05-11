using Alchemie.Core;
using Alchemie.Models;
using Alchemie.Models.Types;

namespace Alchemie.UI.ViewModels
{
    public class CharacterViewModel : ObservableObject
    {
        #region Construction

        public CharacterViewModel()
        {
            character_ = new Character();
        }

        public CharacterViewModel(Character character)
        {
            character_ = character;
        }

        #endregion Construction

        #region Members

        private Character character_;

        private bool usingMhAlchemie_ = false;
        private bool usingMhKochen_ = false;

        private int mhAlchemie_ = 0;
        private int mhKochen_ = 0;

        #endregion Members

        #region Properties

        public Character Character
        {
            get
            {
                return character_;
            }
            set
            {
                character_ = value;
                RaisePropertyChange(null);
            }
        }

        public int MU
        {
            get
            {
                return Character.MU;
            }
            set
            {
                Character.MU = value;
                RaisePropertyChange(nameof(MU));
            }
        }

        public int KL
        {
            get
            {
                return Character.KL;
            }
            set
            {
                Character.KL = value;
                RaisePropertyChange(nameof(KL));
            }
        }

        public int FF
        {
            get
            {
                return Character.FF;
            }
            set
            {
                Character.FF = value;
                RaisePropertyChange(nameof(FF));
            }
        }

        public int IN
        {
            get
            {
                return Character.IN;
            }
            set
            {
                Character.IN = value;
                RaisePropertyChange(nameof(IN));
            }
        }

        public int Alchemie
        {
            get
            {
                return Character.Alchemie;
            }
            set
            {
                Character.Alchemie = value;
                RaisePropertyChange(nameof(Alchemie));
            }
        }

        public int Kochen
        {
            get
            {
                return Character.Kochen;
            }
            set
            {
                Character.Kochen = value;
                RaisePropertyChange(nameof(Kochen));
            }
        }

        public LaborID Labor
        {
            get
            {
                return Character.Labor;
            }
            set
            {
                Character.Labor = value;
                RaisePropertyChange(nameof(Labor));
            }
        }

        public LaborQL LaborQuality
        {
            get
            {
                return Character.LaborQuality;
            }
            set
            {
                Character.LaborQuality = value;
                RaisePropertyChange(nameof(LaborQuality));
            }
        }

        public int AlchemieMH
        {
            get
            {
                return Character.AlchemieMH;
            }
            set
            {
                Character.AlchemieMH = value;
                RaisePropertyChange(nameof(AlchemieMH));
            }
        }

        public int KochenMH
        {
            get
            {
                return Character.KochenMH;
            }
            set
            {
                Character.KochenMH = value;
                RaisePropertyChange(nameof(KochenMH));
            }
        }

        public bool UsingAlchemie
        {
            get
            {
                return Character.UsingAlchemie;
            }
            set
            {
                Character.UsingAlchemie = value;
                RaisePropertyChange(nameof(UsingAlchemie));
            }
        }

        public bool AllegorischeAnalyse
        {
            get
            {
                return Character.AllegorischeAnalyse;
            }
            set
            {
                Character.AllegorischeAnalyse = value;
                RaisePropertyChange(nameof(AllegorischeAnalyse));
            }
        }

        public bool ChymischeHochzeit
        {
            get
            {
                return Character.ChymischeHochzeit;
            }
            set
            {
                Character.ChymischeHochzeit = value;
                RaisePropertyChange(nameof(ChymischeHochzeit));
            }
        }

        public bool MandriconsBindung
        {
            get
            {
                return Character.MandriconsBindung;
            }
            set
            {
                Character.MandriconsBindung = value;
                RaisePropertyChange(nameof(MandriconsBindung));
            }
        }

        public bool UsingMHAlchemie
        {
            get => usingMhAlchemie_;

            set
            {
                usingMhAlchemie_ = value;
                if (value)
                {
                    Character.AlchemieMH = mhAlchemie_;
                }
                else
                {
                    mhAlchemie_ = Character.AlchemieMH;
                    Character.AlchemieMH = 0;
                }
                RaisePropertyChange(nameof(UsingMHAlchemie));
            }
        }

        public bool UsingMHKochen
        {
            get => usingMhKochen_;

            set
            {
                usingMhKochen_ = value;
                if (value)
                {
                    Character.KochenMH = mhKochen_;
                }
                else
                {
                    mhKochen_ = Character.KochenMH;
                    Character.KochenMH = 0;
                }
                RaisePropertyChange(nameof(UsingMHKochen));
            }
        }

        #endregion Properties
    }
}