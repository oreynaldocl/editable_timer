using EditableTimer;
using System;
using System.Threading.Tasks;

namespace TimerDisplayer.Executers
{
    public class SimpleExecuter : ITimerExecuter
    {
        private int _identifier;
        private readonly ILogger _logger;
        private readonly ITimerManager _manager;
        private readonly Random _random;
        private int _errorAfter = 2;
        private int _amountExecution = 0;

        public int Identifier => _identifier;

        public SimpleExecuter(
            int identifier,
            ILogger logger,
            ITimerManager manager)
        {
            _identifier = identifier;
            _logger = logger;
            _manager = manager;
            _random = new Random();

            _manager.RegisterTimer(this, TimeSpan.FromSeconds(10));
        }

        public Task<TimeSpan> CalculateNextTime()
        {
            return Task.FromResult(TimeSpan.FromSeconds(_random.Next(2, 10)));
        }

        public async Task ExecuteHandler()
        {
            if (_amountExecution >= _errorAfter )
            {
                throw new Exception("Forcing an Execution error");
            }
            _amountExecution++;
            _logger.Log($"[{_identifier}] Execution works. Execution #{_amountExecution}. Waiting 2s #####");
            await Task.Delay(2000);
        }

        public Task FailureHandler(Exception ex)
        {
            _manager.UnregisterTimer(this);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _manager.UnregisterTimer(this);
        }

        internal void ChangeTime(TimeSpan timeSpan)
        {
            _manager.ChangeWaitTime(this, timeSpan);
        }
    }
}
