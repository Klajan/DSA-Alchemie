using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Alchemie.Core
{
    abstract public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool EnablePropertyChange { set; get; } = true;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Method to raise event")]
        protected void RaisePropertyChange([CallerMemberName] string name = null)
        {
            if (EnablePropertyChange) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyname = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                return false;
            }
            backingField = value;
            RaisePropertyChange(propertyname);
            return true;
        }
    }
}