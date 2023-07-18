using EditableTimer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TimerDisplayer.Executers
{
    public class SimpleExecuter : ITimerExecuter
    {
        private int identifier = 0;
        private readonly ILogger _logger;
        private readonly Random random = new Random();

        public int Identifier => identifier;

        public TimeSpan DefaultPeriod => TimeSpan.FromSeconds(60);

        public SimpleExecuter(int identifier, ILogger logger)
        {
            this.identifier = identifier;
            _logger = logger;
        }

        public Task<TimeSpan> CalculateNextTime()
        {
            return Task.FromResult(TimeSpan.FromSeconds(random.Next(2, 10)));
        }

        public async Task ExecuteHandler()
        {
            await Task.Delay(500);
            _logger.Log($"[{identifier}] Just testing it works ##########");
        }

        public Task FailureHandler()
        {
            throw new NotImplementedException();
        }
    }
}
