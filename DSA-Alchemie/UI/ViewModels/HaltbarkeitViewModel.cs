using Alchemie.Core;
using Alchemie.Models;
using Alchemie.Models.Types;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Alchemie.UI.ViewModels
{
    public class HaltbarkeitViewModel : ObservableObject
    {
        public HaltbarkeitViewModel()
        {
            ExtendHaltbarkeitCommand = new RelayCommand(o => _trank.HaltbarkeitVerlängern(), o => { return _trank.Quality > Quality.M; });
        }

        public HaltbarkeitViewModel(Trank trank) : this()
        {
            Trank = trank;
        }

        public ICommand ExtendHaltbarkeitCommand
        {
            set; get;
        }

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
                    RaisePropertyChange(nameof(ExpiryBaseValue));
                    RaisePropertyChange(nameof(ExpiryBaseStr));
                    break;

                case nameof(_trank.ExpiryValue):
                    RaisePropertyChange(nameof(ExpiryValue));
                    RaisePropertyChange(nameof(ExpiryExtendedStr));
                    break;

                default:
                    RaisePropertyChange(e.PropertyName);
                    break;
            }
        }

        private Trank _trank = new();

        public Trank Trank
        {
            get
            {
                return _trank;
            }
            set
            {
                if (SetValue(ref _trank, value, null) && value != null)
                {
                    _trank.PropertyChanged += HandlePropertyChanged;
                    _trank.Character.PropertyChanged += HandlePropertyChanged;
                }
            }
        }

        private bool _expiryIsReadOnly = true;

        public bool ExpiryIsReadonly
        {
            get => _expiryIsReadOnly;
            set
            {
                if (SetValue(ref _expiryIsReadOnly, value))
                {
                    RaisePropertyChange(nameof(ExpiryValueMax));
                    RaisePropertyChange(nameof(ExpiryValueMin));
                }
            }
        }

        public int ExpiryFailRoll
        {
            get => _trank.ExpiryFailRoll;
            set => _trank.ExpiryFailRoll = value;
        }

        public int ExpiryBaseValue
        {
            get => _trank.ExpiryBaseValue;
            set => _trank.ExpiryBaseValue = value;
        }

        public int ExpiryValue
        {
            get => _trank.ExpiryValue;
        }

        public int ExpiryValueMax
        {
            get => !_expiryIsReadOnly ? _trank.Rezept.Haltbarkeit.MaxValue : Int32.MaxValue;
        }

        public int ExpiryValueMin
        {
            get => !_expiryIsReadOnly ? _trank.Rezept.Haltbarkeit.MinValue : Int32.MinValue;
        }

        public int TaPStarHaltbarkeit
        {
            get => _trank.TaPStarHaltbarkeit;
        }

        public string ExpiryBaseStr
        {
            get => _trank.Rezept.Haltbarkeit.GetHaltbarkeitStr(ExpiryBaseValue);
        }

        public string ExpiryExtendedStr
        {
            get => _trank.Rezept.Haltbarkeit.GetHaltbarkeitStr(ExpiryValue);
        }

        public string ExpiryResultStr
        {
            get => _trank.ExpiryResultStr;
        }

        public string TimeUnit
        {
            get => _trank.Rezept.Haltbarkeit.TimeUnit;
        }

        public ExtendedObserableCollection<int> HaltbarkeitEigenschaftDice
        {
            get => _trank.HaltbarkeitEigenschaftDice;
        }

        public Quality Quality
        {
            get => _trank.Quality;
            set => _trank.Quality = value;
        }

        public bool UseRNG
        {
            get => _trank.UseRNG;
            set => _trank.UseRNG = value;
        }
    }
}