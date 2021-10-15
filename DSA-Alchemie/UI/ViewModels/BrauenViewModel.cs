using Alchemie.Core;
using Alchemie.Models;
using Alchemie.Models.Types;
using System.ComponentModel;
using System.Windows.Input;

namespace Alchemie.UI.ViewModels
{
    public class BrauenViewModel : ObservableObject
    {
        public ICommand HandleBrauenCommand { set; get; }

        #region Construction

        public BrauenViewModel()
        {
            HandleBrauenCommand = new RelayCommand(o => _trank.Brauen((int)_subsitution + _miscMod, (_zurückhalten, _astralAufladen, _miscQMod)));
        }

        public BrauenViewModel(Character character) : this()
        {
            _trank = new Trank(character);
        }

        public BrauenViewModel(Trank trank) : this()
        {
            _trank = trank;
        }

        #endregion Construction

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case null:
                case nameof(_trank.Rezept):
                case nameof(_trank.Character):
                    RaisePropertyChange(null);
                    break;

                case nameof(_trank.ExpiryBaseValue):
                    RaisePropertyChange(nameof(ExpiryBaseString));
                    break;

                default:
                    RaisePropertyChange(e.PropertyName);
                    break;
            }
        }

        private Trank _trank = new();

        public Trank Trank
        {
            get => _trank;
            set
            {
                if (SetValue(ref _trank, value, null) && _trank != null)
                {
                    _trank.PropertyChanged += HandlePropertyChanged;
                    _trank.Character.PropertyChanged += HandlePropertyChanged;
                }
            }
        }

        public Character Character
        {
            get => _trank.Character;
            set
            {
                if (_trank.Character != value)
                {
                    _trank.Character = value;
                    if (_trank.Character != null)
                    {
                        _trank.Character.PropertyChanged += HandlePropertyChanged;
                    }
                    RaisePropertyChange(null);
                }
            }
        }

        public bool UseRNG
        {
            get => _trank.UseRNG;
            set => _trank.UseRNG = value;
        }

        public Rezept Rezept
        {
            get => _trank.Rezept;
            set
            {
                if (_trank.Rezept != value)
                {
                    _trank.Rezept = value;
                    RaisePropertyChange(null);
                }
            }
        }

        private Subsitution _subsitution;

        public Subsitution Subsitution
        {
            get => _subsitution;
            set => SetValue(ref _subsitution, value);
        }

        private int _zurückhalten;

        public int Zurückhalten
        {
            get => _zurückhalten;
            set => SetValue(ref _zurückhalten, value);
        }

        private int _astralAufladen;

        public int AstralAufladen
        {
            get => _astralAufladen;
            set => SetValue(ref _astralAufladen, value);
        }

        private int _miscMod;

        public int MiscMod
        {
            get => _miscMod;
            set => SetValue(ref _miscMod, value);
        }

        private int _miscQMod;

        public int MiscQMod
        {
            get => _miscQMod;
            set => SetValue(ref _miscQMod, value);
        }

        public ExtendedObserableCollection<int> BrauenEigenschaftDice
        {
            get => _trank.BrauenEigenschaftDice;
        }

        public ExtendedObserableCollection<int> BrauenQualityDice
        {
            get => _trank.BrauenQualityDice;
        }

        public LaborID Labor
        {
            get => Character.Labor;
        }

        public LaborQL LaborQuality
        {
            get => Character.LaborQuality;
        }

        public LaborID RezeptLabor
        {
            get => _trank.Rezept.Labor.ID;
        }

        public int RezeptBrauenMod
        {
            get => _trank.Rezept.Probe.BrauenMod;
        }

        public bool ChymischeHochzeit
        {
            get => _trank.Character.ChymischeHochzeit;
        }

        public string CurrentWirkung
        {
            get => _trank.CurrentWirkung;
        }

        public int TaPStarBrauen
        {
            get => _trank.TaPStarBrauen;
        }

        public string ExpiryBaseString
        {
            get => _trank.Rezept.Haltbarkeit.GetHaltbarkeitStr(_trank.ExpiryBaseValue);
        }

        public Quality Quality
        {
            get => _trank.Quality;
            set => _trank.Quality = value;
        }
    }
}