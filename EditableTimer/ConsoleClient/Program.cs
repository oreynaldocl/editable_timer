using EditableTimer;
using System;
using System.Threading;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger log = new Logger();
            TimerManager manager = new TimerManager(log);

            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} T#{Thread.CurrentThread.ManagedThreadId} Creating an executer");

            SimpleExecuter simple1 = new SimpleExecuter(1, manager);

            Console.ReadLine();
        }
    }
}
