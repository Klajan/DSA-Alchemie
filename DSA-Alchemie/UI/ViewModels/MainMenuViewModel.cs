using Alchemie.Core;
using System.Windows;

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
            get => Alchemie.Properties.Settings.Default.SaveCharacterOnExit;
            set
            {
                if (Alchemie.Properties.Settings.Default.SaveCharacterOnExit != value)
                {
                    Alchemie.Properties.Settings.Default.SaveCharacterOnExit = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool LoadChar
        {
            get => Alchemie.Properties.Settings.Default.LoadCharacterOnStart;
            set
            {
                if (Alchemie.Properties.Settings.Default.LoadCharacterOnStart != value)
                {
                    Alchemie.Properties.Settings.Default.LoadCharacterOnStart = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool CheckUpdates
        {
            get => Alchemie.Properties.Settings.Default.CheckForUpdates;
            set
            {
                if (Alchemie.Properties.Settings.Default.CheckForUpdates != value)
                {
                    Alchemie.Properties.Settings.Default.CheckForUpdates = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool CheckPrerelease
        {
            get => Alchemie.Properties.Settings.Default.CheckForPrerelease;
            set
            {
                if (Alchemie.Properties.Settings.Default.CheckForPrerelease != value)
                {
                    Alchemie.Properties.Settings.Default.CheckForPrerelease = value;
                    RaisePropertyChange();
                }
            }
        }

        #endregion Properties
    }
}