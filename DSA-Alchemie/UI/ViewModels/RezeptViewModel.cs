using Alchemie.common;

namespace Alchemie.UI.ViewModels
{
    public class RezeptViewModel : BaseViewModel
    {
        #region Construction

        public RezeptViewModel()
        {
            rezept_ = new Rezept();
        }

        public RezeptViewModel(Rezept rezept)
        {
            rezept_ = rezept;
        }

        #endregion Construction

        #region Members

        private Rezept rezept_;

        #endregion Members

        #region Properties

        public Rezept Rezept
        {
            get
            {
                return rezept_;
            }
            set
            {
                rezept_ = value;
                RaisePropertyChange(null);
            }
        }

        public string Name
        {
            get
            {
                return Rezept.Name;
            }
        }

        public string Gruppe
        {
            get
            {
                return Rezept.Gruppe;
            }
        }

        public Labor Labor
        {
            get
            {
                return Rezept.Labor;
            }
        }

        public Probe Probe
        {
            get
            {
                return Rezept.Probe;
            }
        }

        public Beschaffung Beschaffung
        {
            get
            {
                return Rezept.Beschaffung;
            }
            set
            {
                Rezept.Beschaffung = value;
                RaisePropertyChange(nameof(Beschaffung));
            }
        }

        public string Verbreitung
        {
            get
            {
                return Rezept.Verbreitung;
            }
            set
            {
                Rezept.Verbreitung = value;
                RaisePropertyChange(nameof(Verbreitung));
            }
        }

        public string Haltbarkeit
        {
            get
            {
                return Rezept.Haltbarkeit;
            }
            set
            {
                Rezept.Haltbarkeit = value;
                RaisePropertyChange(nameof(Haltbarkeit));
            }
        }

        public string Preis
        {
            get
            {
                return Rezept.Preis;
            }
            set
            {
                Rezept.Preis = value;
                RaisePropertyChange(nameof(Preis));
            }
        }

        public string Rezeptur
        {
            get
            {
                return Rezept.Rezeptur;
            }
            set
            {
                Rezept.Rezeptur = value;
                RaisePropertyChange(nameof(Rezeptur));
            }
        }

        public int Seite
        {
            get
            {
                return Rezept.Seite;
            }
            set
            {
                Rezept.Seite = value;
                RaisePropertyChange(nameof(Seite));
            }
        }

        public string Merkmale
        {
            get
            {
                return Rezept.Merkmale;
            }
            set
            {
                Rezept.Merkmale = value;
                RaisePropertyChange(nameof(Merkmale));
            }
        }

        public string Beschreibung
        {
            get
            {
                return Rezept.Beschreibung;
            }
            set
            {
                Rezept.Beschreibung = value;
                RaisePropertyChange(nameof(Beschreibung));
            }
        }

        public string Meisterhinweise
        {
            get
            {
                return Rezept.Meisterhinweise;
            }
            set
            {
                Rezept.Meisterhinweise = value;
                RaisePropertyChange(nameof(Meisterhinweise));
            }
        }

        public Wirkung Wirkung
        {
            get
            {
                return Rezept.Wirkung;
            }
            set
            {
                Rezept.Wirkung = value;
                RaisePropertyChange(nameof(Wirkung));
            }
        }

        #endregion Properties
    }
}