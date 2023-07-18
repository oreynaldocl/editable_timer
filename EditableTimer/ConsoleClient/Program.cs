using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EditableTimer;

namespace ConsoleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Logger log = new Logger();
            TimerManager manager = new TimerManager(log);

            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} T#{Thread.CurrentThread.ManagedThreadId} Creating an executer");
            // CREATION EXECUTER
            SimpleExecuter simple1 = new SimpleExecuter(1, manager);
            List<SimpleExecuter> list = new List<SimpleExecuter>();
            for (int i = 0; i < 15; i++) {
                list.Add(new SimpleExecuter(i + 2, manager));
            }

            Thread.Sleep(1000);
            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} T#{Thread.CurrentThread.ManagedThreadId} Updating to wait 2 secs");
            manager.ChangeWaitTime(simple1, TimeSpan.FromSeconds(2));

            await Task.Delay(35000);
            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} T#{Thread.CurrentThread.ManagedThreadId} Unregister an executor");
            manager.UnregisterTimer(simple1);

            Console.ReadKey();
        }
    }
}
