using Alchemie.common;

namespace Alchemie.UI.ViewModels
{
    public class TrankViewModel : BaseViewModel
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

        #endregion Properties
    }
}