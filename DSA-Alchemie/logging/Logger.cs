using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.ComponentModel;

namespace DSA_Alchemie.logging
{
    public interface ILogger
    {
        void Log(string message);
    }

    class UILogger : ILogger, INotifyPropertyChanged
    {
        private static Mutex lock_;
        static Queue<string> messageQueue;
        string currentLog;

        public event PropertyChangedEventHandler PropertyChanged;

        public UILogger()
        {

        }

        public void Log(string message)
        {
            lock(lock_){
                currentLog = message;
                PropertyChanged(this, new PropertyChangedEventArgs("currentLog"));
            }
        }
    }
}


