using EditableTimer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient
{
    public class SimpleExecuter : ITimerExecuter
    {
        private readonly ITimerManager _manager;
        private readonly Random _random;

        private int identifier;
        public int Identifier => identifier;

        public TimeSpan DefaultPeriod => TimeSpan.FromSeconds(5);

        public SimpleExecuter(int identifier, ITimerManager manager)
        {
            this.identifier = identifier;
            _manager = manager;
            _random = new Random();

            _manager.RegisterTimer(this, TimeSpan.FromSeconds(10));
        }

        public Task<TimeSpan> CalculateNextTime()
        {
            return Task.FromResult(TimeSpan.FromSeconds(_random.Next(5, 15)));
        }

        public async Task ExecuteHandler()
        {
            await Task.Delay(500);
            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} [{identifier}-T#{Thread.CurrentThread.ManagedThreadId}] EXECUTING Just execute a simple log ###");
        }

        public Task FailureHandler()
        {
            throw new NotImplementedException();
        }
    }
}
