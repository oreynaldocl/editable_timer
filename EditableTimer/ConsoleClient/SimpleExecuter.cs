using EditableTimer;
using System;
using System.Threading;
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
        private readonly Random _random;

        private int identifier;
        public int Identifier => identifier;

        private int executionCounter = 0;
        private int errroFrom = 5;

        private int incrementalAttempt = 0;
        private int[] incrementalTime = new int[] { 100, 200, 4000, 800, 1600, 3200, 6400, 12800, 25600, 51200, 102400 };

        public SimpleExecuter(int identifier, ITimerManager manager)
        {
            this.identifier = identifier;
            _manager = manager;
            _random = new Random();

            _manager.RegisterTimer(this, TimeSpan.FromSeconds(2));
        }

        public Task<TimeSpan> CalculateNextTime()
        {
            return Task.FromResult(TimeSpan.FromSeconds(_random.Next(2, 5)));
        }

        public async Task ExecuteHandler()
        {
            if (executionCounter >= errroFrom)
                throw new Exception("Forcing an error for start a failover process");
            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.ffff")} [{identifier}-T#{Thread.CurrentThread.ManagedThreadId}] EXECUTING #{executionCounter} Just execute a simple log ###");
            executionCounter++;
            await Task.Delay(500);
        }

        public Task FailureHandler()
        {
            TimeSpan timeSpan = BuildIncrementalPeriod(incrementalAttempt);
            incrementalAttempt++;

            _manager.ChangeWaitTime(this, timeSpan);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _manager.UnregisterTimer(this);
        }

        internal TimeSpan BuildIncrementalPeriod(int retryCounter)
        {
            int periodMs = 120000;
            if (retryCounter < incrementalTime.Length)
                periodMs = incrementalTime[retryCounter];

            return TimeSpan.FromMilliseconds(periodMs);
        }
    }
}
