using System;
using EditableTimer;

namespace ConsoleClient
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            string timeStr = DateTime.UtcNow.ToString("HH:mm:ss.ff");
            Console.WriteLine($"{timeStr} {message}");
        }

        public void LogError(string message, Exception ex)
        {
            string timeStr = DateTime.UtcNow.ToString("HH:mm:ss.ff");
            Console.WriteLine($"{timeStr} ERROR !!!! {message}. Exception: {ex.Message} !!!!");
        }
    }
}
