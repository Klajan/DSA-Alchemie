using Alchemie.Core;
using Alchemie.Models;
using System.ComponentModel;

namespace Alchemie.UI.ViewModels
{
    public class TrankViewModel : ObservableObject
    {
        #region Construction

        public TrankViewModel()
        {
        }

        public TrankViewModel(Trank trank) : this()
        {
            trank_ = trank;
        }

        #endregion Construction

        #region Functions

        private void TrankModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChange(e.PropertyName);
        }

        #endregion Functions

        #region Members

        private Trank trank_ = new Trank();

        #endregion Members

        #region Properties

        public Trank Trank
        {
            get => trank_;
            set
            {
                trank_ = value;
                if (trank_ != null)
                {
                    trank_.PropertyChanged += TrankModel_PropertyChanged;
                }
                RaisePropertyChange(null);
            }
        }

        public ExtendedObserableCollection<int> EigenschaftDice
        {
            get => trank_.EigenschaftDice;
        }

        public ExtendedObserableCollection<int> QualityDice
        {
            get => trank_.QualityDice;
        }

        public char Quality
        {
            get => trank_.Quality;
            set
            {
                trank_.Quality = value;
                RaisePropertyChange(nameof(Quality));
            }
        }

        public string CurrentWirkung
        {
            get => trank_.CurrentWirkung;
        }

        #endregion Properties
    }
}