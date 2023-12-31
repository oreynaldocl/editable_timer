﻿using EditableTimer;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimerDisplayer.Executers
{
    public class LoggerText : ILogger, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _completeLog = "## See Complete Log: ##";

        public string CompleteLog
        {
            get { return _completeLog; }
            set {
                if (_completeLog != value)
                {
                    _completeLog = value;
                    OnPropertyChanged();
                }
            }
        }


        public void Log(string message)
        {
            CompleteLog += $"\n{DateTime.UtcNow:mm:ss.fff} {message}";
        }

        public void LogError(string message, Exception ex)
        {
            CompleteLog += $"\n{DateTime.UtcNow:mm:ss.fff} ERROR {message}. Exception {ex.Message}";
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
