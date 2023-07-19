using System;

namespace EditableTimer
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(string message, Exception ex);
    }
}
