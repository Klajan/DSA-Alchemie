using Alchemie.Core;
using Alchemie.Models;
using Alchemie.Models.Types;
using System;
using System.ComponentModel;

namespace Alchemie.UI.ViewModels
{
    public class RezeptViewModel : ObservableObject
    {
        #region Construction

        public RezeptViewModel()
        {
            _rezept = new Rezept();
        }

        public RezeptViewModel(Rezept rezept)
        {
            if (rezept == null) throw new ArgumentNullException(nameof(rezept));
            _rezept = rezept;
        }

        public RezeptViewModel(Trank trank)
        {
            if (trank == null) throw new ArgumentNullException(nameof(trank));
            _rezept = trank.Rezept;
        }

        #endregion Construction

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Rezept):
                    RaisePropertyChange(null);
                    break;

                default:
                    RaisePropertyChange(e.PropertyName);
                    break;
            }
        }

        #region Members

        private Rezept _rezept;

        #endregion Members

        #region Properties

        public Rezept Rezept
        {
            get => _rezept;
            set => SetValue(ref _rezept, value, null);
        }

        public string Name
        {
            get => Rezept.Name;
        }

        public string Gruppe
        {
            get => Rezept.Gruppe;
        }

        public Labor Labor
        {
            get => Rezept.Labor;
        }

        public Probe Probe
        {
            get => Rezept.Probe;
        }

        public Beschaffung Beschaffung
        {
            get => Rezept.Beschaffung;
            set
            {
                Rezept.Beschaffung = value;
                RaisePropertyChange();
            }
        }

        public string Verbreitung
        {
            get => Rezept.Verbreitung;
            set
            {
                Rezept.Verbreitung = value;
                RaisePropertyChange();
            }
        }

        public string Haltbarkeit
        {
            get => Rezept.Haltbarkeit.ToString();
            set
            {
                if (Rezept.Haltbarkeit.ToString() != value)
                {
                    Rezept.Haltbarkeit = new(value);
                    RaisePropertyChange();
                }
            }
        }

        public string Preis
        {
            get => Rezept.Preis;
            set
            {
                if (Rezept.Preis != value)
                {
                    Rezept.Preis = value;
                    RaisePropertyChange();
                }
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
                if (Rezept.Rezeptur != value)
                {
                    Rezept.Rezeptur = value;
                    RaisePropertyChange();
                }
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
                if (Rezept.Seite != value)
                {
                    Rezept.Seite = value;
                    RaisePropertyChange();
                }
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
                if (Rezept.Merkmale != value)
                {
                    Rezept.Merkmale = value;
                    RaisePropertyChange();
                }
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
                if (Rezept.Beschreibung != value)
                {
                    Rezept.Beschreibung = value;
                    RaisePropertyChange();
                }
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
                if (Rezept.Meisterhinweise != value)
                {
                    Rezept.Meisterhinweise = value;
                    RaisePropertyChange();
                }
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
                if (Rezept.Wirkung != value)
                {
                    Rezept.Wirkung = value;
                    RaisePropertyChange();
                }
            }
        }

        #endregion Properties

        public void ChangeRezept(object sender, Rezept newRezept)
        {
            Rezept = newRezept;
        }
    }
}