using Alchemie.common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
        #endregion


        #region Members
        private Trank trank_ = new Trank();
        #endregion

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
        #endregion
        
    }
}
