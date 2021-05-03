using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Alchemie.Core;

namespace Alchemie.UI.ViewModels
{
    public class MainMenuViewModel : ObservableObject
    {
        #region Construction

        public MainMenuViewModel()
        {
            app_ = Application.Current as App;
        }

        #endregion Construction

        #region Members

        private readonly App app_;

        #endregion Members

        #region Properties

        public bool SaveChar
        {
            get { return Alchemie.Properties.Settings.Default.SaveCharacterOnExit; }
            set { Alchemie.Properties.Settings.Default.SaveCharacterOnExit = value; RaisePropertyChange(); }
        }

        public bool LoadChar
        {
            get { return Alchemie.Properties.Settings.Default.LoadCharacterOnStart; }
            set { Alchemie.Properties.Settings.Default.LoadCharacterOnStart = value; RaisePropertyChange(); }
        }

        public bool CheckUpdates
        {
            get { return Alchemie.Properties.Settings.Default.CheckForUpdates; }
            set { Alchemie.Properties.Settings.Default.CheckForUpdates = value; RaisePropertyChange(); }
        }

        public bool CheckPrerelease
        {
            get { return Alchemie.Properties.Settings.Default.CheckForPrerelease; }
            set { Alchemie.Properties.Settings.Default.CheckForPrerelease = value; RaisePropertyChange(); }
        }


        #endregion Properties

    }
}
