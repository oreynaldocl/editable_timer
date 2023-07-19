using EditableTimer;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TimerDisplayer.Executers
{
    public class WorkerExecuter : ITimerExecuter
    {
        private int identifier = -1;
        private readonly ILogger _logger;
        private readonly Dispatcher _dispatcher;
        public readonly StackPanel panel;
        public int interval { get; set; } = 15;
        private Boolean isTriggered = false;

        public int Identifier => identifier;

        public TimeSpan DefaultPeriod => TimeSpan.FromSeconds(60);

        public WorkerExecuter(ILogger logger, Dispatcher dispatcher, StackPanel panel, int identifier, int interval)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            this.panel = panel;
            this.identifier = identifier;
            this.interval = interval;
        }

        public Task<TimeSpan> CalculateNextTime()
        {
            if(isTriggered)
            {
                return Task.FromResult(TimeSpan.FromSeconds(3));
            }
            return Task.FromResult(TimeSpan.FromSeconds(interval));
        }

        public Task ExecuteHandler()
        {
            _dispatcher.Invoke(() => {
                if (isTriggered == false)
                {
                    isTriggered = true;
                    ((Label)panel.Children[5]).Content = "triggered";
                }
                else
                {
                    isTriggered = false;
                    ((Label)panel.Children[5]).Content = "";
                }
            });
            return Task.CompletedTask;
        }

        public Task FailureHandler()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
