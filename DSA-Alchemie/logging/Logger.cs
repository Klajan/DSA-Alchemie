﻿using System.Collections.Generic;
using System.ComponentModel;

#pragma warning disable // #warning directive

namespace Alchemie.logging
{
    public interface ILogger
    {
        void Log(string message);
    }

    internal class UILogger : ILogger, INotifyPropertyChanged
    {
        private static object Locker { set; get; } = new object();
        private static Queue<string> messageQueue;
        private string currentLog;

        public event PropertyChangedEventHandler PropertyChanged;

        public UILogger()
        {
        }

        public void Log(string message)
        {
            lock (Locker)
            {
                currentLog = message;
                PropertyChanged(this, new PropertyChangedEventArgs("currentLog"));
            }
        }
    }
}