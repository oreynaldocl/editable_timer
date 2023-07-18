using EditableTimer.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EditableTimer
{
    public class TimerManager : ITimerManager
    {
        internal Dictionary<int, TimerItem> timers = new Dictionary<int, TimerItem>();
        private readonly ILogger _logger;

        public TimerManager(ILogger logger)
        {
            _logger = logger;
        }

        public void RegisterTimer(ITimerExecuter executer, TimeSpan dueInitialTime)
        {
            if (timers.ContainsKey(executer.Identifier))
                throw new InvalidOperationException($"Not possible to register again the executer with identifier: {executer.Identifier}");
            dueInitialTime.CheckPositive();
            timers.Add(executer.Identifier, new TimerItem() { Executer = executer });

            _logger.Log($"[{executer.Identifier}] RegisterTimer {dueInitialTime}s");
            StartTimer(executer, DateTime.UtcNow.Add(dueInitialTime));
        }

        public void UnregisterTimer(ITimerExecuter executer)
        {
            throw new NotImplementedException();
        }

        internal void StartTimer(ITimerExecuter executer, DateTime waitUntil)
        {
            UpsertCancellationSource(executer, waitUntil);

            Task.Factory.StartNew(WaitAndExecute, timers[executer.Identifier]);
        }

        internal async Task WaitAndExecute(object param)
        {
            TimerItem timer = (TimerItem)param;
            CancellationToken token = timer.Source.Token;
            DateTime waitUntil = timer.WaitUntil;
            ITimerExecuter executer = timer.Executer;
            try
            {
                while (
                    DateTime.UtcNow <= waitUntil
                    && !token.IsCancellationRequested
                )
                {
                    await Task.Delay(500);
                }
                if (!token.IsCancellationRequested)
                {
                    await executer.ExecuteHandler();
                    TimeSpan timeSpan = await executer.CalculateNextTime();
                    _logger.Log($"[{executer.Identifier}] Next executing in {timeSpan}s");
                    // Method will create new thread
                    StartTimer(executer, DateTime.UtcNow.Add(timeSpan));
                }
                else {
                    _logger.Log($"[{executer.Identifier}] cancelled.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
                await executer.FailureHandler();

                // TODO Probably we don't need to start a timer, probably it will loop forever. I think user can use ChangeWaitTime internally to start again
                TimeSpan timeSpan = await executer.CalculateNextTime();
                // Method will create new thread
                StartTimer(executer, DateTime.UtcNow.Add(timeSpan));
            }
        }

        internal void UpsertCancellationSource(ITimerExecuter executer, DateTime waitUntil)
        {
            timers[executer.Identifier] = new TimerItem()
            {
                // Be sure to have just a new Source, executer will be the same
                Executer = timers[executer.Identifier].Executer,
                Source = new CancellationTokenSource(),
                WaitUntil = waitUntil,
            };
        }

        public void ChangeWaitTime(ITimerExecuter executer, TimeSpan newTime)
        {
            if (!timers.ContainsKey(executer.Identifier))
                throw new Exception($"Not found executer with identifier: {executer.Identifier}");
            newTime.CheckPositive();

            timers[executer.Identifier].Source.Cancel();
            StartTimer(executer, DateTime.UtcNow.Add(newTime));
            _logger.Log($"[{executer.Identifier}] ChangeWaitTime {newTime}s");
        }

    }
}
