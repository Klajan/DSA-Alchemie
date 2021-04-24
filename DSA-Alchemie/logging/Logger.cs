using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.ComponentModel;

namespace Alchemie.logging
{
    public interface ILogger
    {
        void Log(string message);
    }

    class UILogger : ILogger, INotifyPropertyChanged
    {
        private static object Locker { set; get; } = new object();
        static Queue<string> messageQueue;
        string currentLog;

        public event PropertyChangedEventHandler PropertyChanged;

        public UILogger()
        {

        }

        public void Log(string message)
        {
            lock(Locker){
                currentLog = message;
                PropertyChanged(this, new PropertyChangedEventArgs("currentLog"));
            }
        }
    }
}


