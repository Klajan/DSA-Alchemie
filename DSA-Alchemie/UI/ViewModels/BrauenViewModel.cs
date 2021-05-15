﻿using Alchemie.Core;
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

        private void BrauenViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        private Trank _trank = new();

        public Trank Trank
        {
            get => _trank;
            set
            {
                _trank = value;
                if (_trank != null)
                {
                    _trank.PropertyChanged += BrauenViewModel_PropertyChanged;
                    _trank.Character.PropertyChanged += BrauenViewModel_PropertyChanged;
                }
                RaisePropertyChange(null);
            }
        }

        public Character Character
        {
            get => _trank.Character;
            set
            {
                _trank.Character = value;
                if (_trank.Character != null)
                {
                    _trank.Character.PropertyChanged += BrauenViewModel_PropertyChanged;
                }
                RaisePropertyChange(null);
            }
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

        public Rezept Rezept
        {
            get => _trank.Rezept;
            set
            {
                _trank.Rezept = value;
                RaisePropertyChange(null);
            }
        }

        private Subsitution _subsitution;

        public Subsitution Subsitution
        {
            get { return _subsitution; }
            set
            {
                _subsitution = value;
                RaisePropertyChange();
            }
        }

        private int _zurückhalten;

        public int Zurückhalten
        {
            get { return _zurückhalten; }
            set
            {
                _zurückhalten = value;
                RaisePropertyChange();
            }
        }

        private int _astralAufladen;

        public int AstralAufladen
        {
            get { return _astralAufladen; }
            set
            {
                _astralAufladen = value;
                RaisePropertyChange();
            }
        }

        private int _miscMod;

        public int MiscMod
        {
            get { return _miscMod; }
            set
            {
                _miscMod = value;
                RaisePropertyChange();
            }
        }

        private int _miscQMod;

        public int MiscQMod
        {
            get { return _miscQMod; }
            set
            {
                _miscQMod = value;
                RaisePropertyChange();
            }
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
            get => _trank.ExpiryBaseString;
        }

        public Quality Quality
        {
            get => _trank.Quality;
            set
            {
                _trank.Quality = value;
                RaisePropertyChange();
            }
        }
    }
}