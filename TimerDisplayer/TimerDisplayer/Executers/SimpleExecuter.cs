using EditableTimer;
using System;
using System.Threading.Tasks;

namespace TimerDisplayer.Executers
{
    public class SimpleExecuter : ITimerExecuter
    {
        private int identifier = 0;
        private readonly ILogger _logger;

        public int Identifier => identifier;

        public TimeSpan DefaultPeriod => TimeSpan.FromSeconds(60);

        public SimpleExecuter(ILogger logger)
        {
            _logger = logger;
        }

        public Task<TimeSpan> CalculateNextTime()
        {
            return Task.FromResult(TimeSpan.FromSeconds(10));
        }

        public Task ExecuteHandler()
        {
            _logger.Log("Just testing it works");
            return Task.CompletedTask;
        }

        public Task FailureHandler()
        {
            throw new NotImplementedException();
        }
    }
}
