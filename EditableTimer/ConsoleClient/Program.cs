using System;
using EditableTimer;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger log = new Logger();
            TimerManager manager = new TimerManager(log);

            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} Creating an executer");
            // CREATION EXECUTER
            SimpleExecuter simple = new SimpleExecuter(1, manager);

            Console.ReadKey();
        }
    }
}
