using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Alchemie.Core;
using Alchemie.Models;
using Alchemie.Models.Types;

namespace Alchemie.UI.ViewModels
{
    public class HaltbarkeitViewModel : ObservableObject
    {

        public HaltbarkeitViewModel(Trank trank)
        {
            _trank = trank;
        }

        public ICommand HaltbarkeitCommand { set; get; }

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

        private Trank _trank;

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

        public int ExpiryValue
        {
            get => _trank.ExpiryValue;
        }

        public int ExpiryValueMax
        {
            get => _trank.Rezept.Haltbarkeit.Dice.MaxRoll;
        }
        
        public int ExpiryValueMin
        {
            get => _trank.Rezept.Haltbarkeit.Dice.MinRoll;
        }

        public string ExpiryString
        {
            get => _trank.ExpiryString;
        }

        public string ExpiryFailedStr
        {
            get => _trank.ExpiryFailedStr;
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

    }
}
