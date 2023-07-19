using EditableTimer;
using System;
using System.Threading;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger();
            TimerManager manager = new TimerManager(logger);

            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ff")} MAIN THREAD Creating an executer");

            SimpleExecuter simple1 = new SimpleExecuter(1, manager, logger);

            Console.ReadLine();
        }
    }
}
