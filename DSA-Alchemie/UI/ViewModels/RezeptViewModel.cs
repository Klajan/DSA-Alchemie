using Alchemie.Core;
using Alchemie.Models;
using System;
using System.ComponentModel;

namespace Alchemie.UI.ViewModels
{
    public class RezeptViewModel : ObservableObject
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

        public RezeptViewModel(Trank trank)
        {
            if (trank == null) throw new ArgumentNullException(nameof(trank));
            rezept_ = trank.Rezept;
        }

        #endregion Construction

        private void RezeptViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Rezept":
                    RaisePropertyChange(null);
                    break;

                default:
                    RaisePropertyChange(e.PropertyName);
                    break;
            }
        }

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
                RaisePropertyChange();
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
                RaisePropertyChange();
            }
        }

        public string Haltbarkeit
        {
            get
            {
                return Rezept.Haltbarkeit.ToString();
            }
            set
            {
                Rezept.Haltbarkeit = new(value);
                RaisePropertyChange();
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
                RaisePropertyChange();
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
                RaisePropertyChange();
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
                RaisePropertyChange();
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
                RaisePropertyChange();
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
                RaisePropertyChange();
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
                RaisePropertyChange();
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
                RaisePropertyChange();
            }
        }

        #endregion Properties

        public void ChangeRezept(object sender, Rezept newRezept)
        {
            Rezept = newRezept;
        }
    }
}