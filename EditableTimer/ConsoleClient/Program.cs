using System;
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

            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} Creating an executer");
            // CREATION EXECUTER
            SimpleExecuter simple = new SimpleExecuter(1, manager);

            await Task.Delay(1000);
            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} Updating to wait 2 secs");
            manager.ChangeWaitTime(simple, TimeSpan.FromSeconds(2));

            await Task.Delay(4000);
            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} Updating to wait 15 secs");
            manager.ChangeWaitTime(simple, TimeSpan.FromSeconds(15));

            Console.ReadKey();
        }
    }
}
