using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Alchemie.Core
{
#pragma warning disable CA1030 // Nach Möglichkeit Ereignisse verwenden
    abstract public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected void RaisePropertyChange([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
