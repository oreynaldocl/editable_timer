using System;
using EditableTimer;

namespace ConsoleClient
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            string timeStr = DateTime.UtcNow.ToString("HH:mm:ss.ffff");
            Console.WriteLine($"{timeStr} - {message}");
        }
    }
}
