using EditableTimer;
using System;
using System.Threading.Tasks;

namespace ConsoleClient
{
    /// <summary>
    /// Example class that just logs message
    /// And simulate errors.
    /// Class will try run an incremental failover
    /// </summary>
    public class SimpleExecuter : ITimerExecuter
    {
        private readonly ITimerManager _manager;
        private readonly ILogger _logger;
        private readonly Random _random;

        private int _identifier;
        public int Identifier => _identifier;

        private int executionCounter = -1;
        private int errroFrom = 5;

        private int errorAttempt = 0;
        private int[] incrementalTime = new int[] { 100, 200, 400, 800, 1600, 3200, 6400, 12800, 25600, 51200, 102400 };

        public SimpleExecuter(
            int identifier,
            ITimerManager manager,
            ILogger logger)
        {
            _identifier = identifier;
            _manager = manager;
            _logger = logger;
            _random = new Random();

            _manager.RegisterTimer(this, TimeSpan.FromSeconds(2));
        }

        internal TimeSpan BuildIncrementalPeriod(int retryCounter)
        {
            int periodMs = 120000;
            if (retryCounter < incrementalTime.Length)
                periodMs = incrementalTime[retryCounter];

            return TimeSpan.FromMilliseconds(periodMs);
        }

        public Task<TimeSpan> CalculateNextTime()
        {
            return Task.FromResult(TimeSpan.FromSeconds(_random.Next(2, 5)));
        }

        public async Task ExecuteHandler()
        {
            executionCounter++;
            _logger.Log($"[{_identifier}] EXECUTE #{executionCounter}. Open a GRPC Connection");
            await Task.Delay(500);
            if (executionCounter >= errroFrom)
            {
                await Task.Delay(1500);
                throw new GrpcConnectionException($"[{_identifier}] EXECUTE #{executionCounter} Not possible to connect to GRPC Server");
            }
            _logger.Log($"[{_identifier}] EXECUTE #{executionCounter}. Request and process data");
            await Task.Delay(500);
        }

        public Task FailureHandler(Exception ex)
        {
            if (ex is GrpcConnectionException)
            {
                TimeSpan timeSpan = BuildIncrementalPeriod(errorAttempt);
                errorAttempt++;

                _manager.ChangeWaitTime(this, timeSpan);
            }
            else
            {
                _manager.UnregisterTimer(this);
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _manager.UnregisterTimer(this);
        }
    }
}
