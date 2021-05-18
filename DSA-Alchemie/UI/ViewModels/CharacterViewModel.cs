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

        private bool usingMhAlchemie_;
        private bool usingMhKochen_;

        private int mhAlchemie_;
        private int mhKochen_;

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
            get => Character.MU; set
            {
                if (Character.MU != value)
                {
                    Character.MU = value;
                    RaisePropertyChange();
                }
            }
        }

        public int KL
        {
            get => Character.KL; set
            {
                if (Character.KL != value)
                {
                    Character.KL = value;
                    RaisePropertyChange();
                }
            }
        }

        public int FF
        {
            get => Character.FF; set
            {
                if (Character.FF != value)
                {
                    Character.FF = value;
                    RaisePropertyChange();
                }
            }
        }

        public int IN
        {
            get => Character.IN; set
            {
                if (Character.IN != value)
                {
                    Character.IN = value;
                    RaisePropertyChange();
                }
            }
        }

        public int Alchemie
        {
            get => Character.Alchemie; set
            {
                if (Character.Alchemie != value)
                {
                    Character.Alchemie = value;
                    RaisePropertyChange();
                }
            }
        }

        public int Kochen
        {
            get => Character.Kochen; set
            {
                if (Character.Kochen != value)
                {
                    Character.Kochen = value;
                    RaisePropertyChange();
                }
            }
        }

        public LaborID Labor
        {
            get => Character.Labor;
            set
            {
                if (Character.Labor != value)
                {
                    Character.Labor = value;
                    RaisePropertyChange();
                }
            }
        }

        public LaborQL LaborQuality
        {
            get => Character.LaborQuality;
            set
            {
                if (Character.LaborQuality != value)
                {
                    Character.LaborQuality = value;
                    RaisePropertyChange();
                }
            }
        }

        public int AlchemieMH
        {
            get => Character.AlchemieMH;
            set
            {
                if (Character.AlchemieMH != value)
                {
                    Character.AlchemieMH = value;
                    RaisePropertyChange();
                }
            }
        }

        public int KochenMH
        {
            get => Character.KochenMH;
            set
            {
                if (Character.KochenMH != value)
                {
                    Character.KochenMH = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool UsingAlchemie
        {
            get => Character.UsingAlchemie;
            set
            {
                if (Character.UsingAlchemie != value)
                {
                    Character.UsingAlchemie = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool AllegorischeAnalyse
        {
            get => Character.AllegorischeAnalyse;
            set
            {
                if (Character.AllegorischeAnalyse != value)
                {
                    Character.AllegorischeAnalyse = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool ChymischeHochzeit
        {
            get => Character.ChymischeHochzeit;
            set
            {
                if (Character.ChymischeHochzeit != value)
                {
                    Character.ChymischeHochzeit = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool MandriconsBindung
        {
            get => Character.MandriconsBindung;
            set
            {
                if (Character.MandriconsBindung != value)
                {
                    Character.MandriconsBindung = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool UsingMHAlchemie
        {
            get => usingMhAlchemie_;

            set
            {
                if (SetValue(ref usingMhAlchemie_, value))
                {
                    if (value)
                    {
                        Character.AlchemieMH = mhAlchemie_;
                    }
                    else
                    {
                        mhAlchemie_ = Character.AlchemieMH;
                        Character.AlchemieMH = 0;
                    }
                }
            }
        }

        public bool UsingMHKochen
        {
            get => usingMhKochen_;

            set
            {
                if (SetValue(ref usingMhKochen_, value))
                {
                    if (value)
                    {
                        Character.KochenMH = mhKochen_;
                    }
                    else
                    {
                        mhKochen_ = Character.KochenMH;
                        Character.KochenMH = 0;
                    }
                }
            }
        }

        #endregion Properties
    }
}