using EditableTimer.Extensions;
using EditableTimer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EditableTimer
{
    public class TimerManager : ITimerManager
    {
        internal Dictionary<int, TimerItem> _timers = new Dictionary<int, TimerItem>();
        private readonly ILogger _logger;

        private IDateTimeWrapper _dateWrapper;
        internal IDateTimeWrapper DateWrapper
        {
            get { return _dateWrapper ?? new DateTimeWrapper(); }
            set { _dateWrapper = value; }
        }

        private ITaskFactory _taskFactory;
        internal ITaskFactory TaskFactory
        {
            get { return _taskFactory ?? new TaskFactoryWrapper(); }
            set { _taskFactory = value; }
        }

        public TimerManager(ILogger logger)
        {
            _logger = logger ?? new DefaultLogger();
        }

        public void RegisterTimer(ITimerExecuter executer, TimeSpan dueInitialTime)
        {
            executer.CheckNull<ArgumentException>("Executer should be defined");
            if (_timers.ContainsKey(executer.Identifier))
                throw new InvalidOperationException($"Not possible to register again the executer with identifier: {executer.Identifier}");
            dueInitialTime.CheckPositive();
            _timers.Add(executer.Identifier, new TimerItem() { Executer = executer });

            _logger.Log($"{BuildId(executer.Identifier)} RegisterTimer {dueInitialTime.TotalSeconds}s");
            StartTimer(executer, DateWrapper.UtcNow.Add(dueInitialTime));
        }

        internal string BuildId(int identifier)
        {
            return $"[{identifier}-T#{Thread.CurrentThread.ManagedThreadId}]";
        }

        public void UnregisterTimer(ITimerExecuter executer)
        {
            executer.CheckNull<ArgumentException>("Executer should be defined");
            if (!_timers.ContainsKey(executer.Identifier))
                throw new Exception($"Not found executer with identifier: {executer.Identifier}");
            _timers[executer.Identifier].Source.Cancel();

            _logger.Log($"{BuildId(executer.Identifier)} UnregisterTimer");
            _timers.Remove(executer.Identifier);
        }

        internal void StartTimer(ITimerExecuter executer, DateTime waitUntil)
        {
            UpdateWaitAndCancellation(executer, waitUntil);

            TaskFactory.StartNew(WaitAndExecute, _timers[executer.Identifier]);
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
                    DateWrapper.UtcNow <= waitUntil
                    && !token.IsCancellationRequested
                )
                {
                    await Task.Delay(500);
                }
                if (!token.IsCancellationRequested)
                {
                    await executer.ExecuteHandler();
                    TimeSpan timeSpan = await executer.CalculateNextTime();
                    _logger.Log($"{BuildId(executer.Identifier)} next execution in {timeSpan.TotalSeconds}s");
                    // Method will create new thread
                    StartTimer(executer, DateWrapper.UtcNow.Add(timeSpan));
                }
                else
                {
                    _logger.Log($"{BuildId(executer.Identifier)} wait until {waitUntil.ToString("mm:ss.fff")} cancelled");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{BuildId(executer.Identifier)} error while executing", ex);
                await executer.FailureHandler(ex);
            }
        }

        internal void UpdateWaitAndCancellation(ITimerExecuter executer, DateTime waitUntil)
        {
            _timers[executer.Identifier] = new TimerItem()
            {
                // Be sure to have just a new Source, executer will be the same
                Executer = _timers[executer.Identifier].Executer,
                Source = new CancellationTokenSource(),
                WaitUntil = waitUntil,
            };
        }

        public void ChangeWaitTime(ITimerExecuter executer, TimeSpan newTime)
        {
            if (!_timers.ContainsKey(executer.Identifier))
                throw new Exception($"Not found executer with identifier: {executer.Identifier}");
            newTime.CheckPositive();

            _timers[executer.Identifier].Source.Cancel();
            StartTimer(executer, DateWrapper.UtcNow.Add(newTime));
            _logger.Log($"{BuildId(executer.Identifier)} ChangeWaitTime {newTime.TotalSeconds}s");
        }

    }
}
