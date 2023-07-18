using EditableTimer;
using System;
using System.Threading.Tasks;

namespace ConsoleClient
{
    public class SimpleExecuter : ITimerExecuter
    {
        private readonly ITimerManager _manager;

        private int identifier;
        public int Identifier => identifier;

        public TimeSpan DefaultPeriod => TimeSpan.FromSeconds(5);

        public SimpleExecuter(int identifier, ITimerManager manager)
        {
            this.identifier = identifier;
            _manager = manager;

            _manager.RegisterTimer(this, TimeSpan.FromSeconds(10));
        }

        public Task<TimeSpan> CalculateNextTime()
        {
            return Task.FromResult(TimeSpan.FromSeconds(10));
        }

        public Task ExecuteHandler()
        {
            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} Just execute a simple log");
            return Task.CompletedTask;
        }

        public Task FailureHandler()
        {
            throw new NotImplementedException();
        }
    }
}
