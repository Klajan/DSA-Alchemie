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
            _trank = trank;
        }

        public ICommand ExtendHaltbarkeitCommand { set; get; }

        private void HaltbarkeitViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Rezept":
                    RaisePropertyChange(null);
                    break;

                case "Character":
                    RaisePropertyChange(null);
                    break;

                default:
                    RaisePropertyChange(e.PropertyName);
                    break;
            }
        }

        private Trank _trank = new Trank();

        public Trank Trank
        {
            get { return _trank; }
            set
            {
                _trank = value;
                if (_trank != null)
                {
                    _trank.PropertyChanged += HaltbarkeitViewModel_PropertyChanged;
                    _trank.Character.PropertyChanged += HaltbarkeitViewModel_PropertyChanged;
                }
                RaisePropertyChange(null);
            }
        }

        private bool _expiryIsReadOnly = true;

        public bool ExpiryIsReadonly
        {
            get { return _expiryIsReadOnly; }
            set { _expiryIsReadOnly = value; RaisePropertyChange(); RaisePropertyChange(nameof(ExpiryValueMax)); RaisePropertyChange(nameof(ExpiryValueMin)); }
        }

        public int ExpiryFailRoll
        {
            get => _trank.ExpiryFailRoll;
        }

        public int ExpiryBaseValue
        {
            get => _trank.ExpiryBaseValue;
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

        public string ExpiryBaseString
        {
            get => _trank.ExpiryBaseString;
        }

        public string ExpiryExtendedString
        {
            get => _trank.ExpiryExtendedString;
        }

        public string ExpiryResultString
        {
            get => _trank.ExpiryResultString;
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
            set { _trank.Quality = value; RaisePropertyChange(); }
        }

        public bool UseRNG
        {
            get { return _trank.UseRNG; }
            set
            {
                _trank.UseRNG = value;
                RaisePropertyChange();
            }
        }
    }
}