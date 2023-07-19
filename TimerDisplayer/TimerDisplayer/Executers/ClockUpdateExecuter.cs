using EditableTimer;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TimerDisplayer.Executers
{
    public class ClockUpdateExecuter : ITimerExecuter
    {
        private int identifier = 1;
        private readonly ILogger _logger;
        private readonly Dispatcher _dispatcher;
        private readonly Label _label;

        public int Identifier => identifier;

        public ClockUpdateExecuter(ILogger logger, Dispatcher dispatcher, Label label)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _label = label;
        }

        public Task<TimeSpan> CalculateNextTime()
        {
            return Task.FromResult(TimeSpan.FromSeconds(1));
        }

        public Task ExecuteHandler()
        {
            _dispatcher.Invoke(() => {
                int currentTime;
                currentTime = Int32.Parse(_label.Content.ToString());
                currentTime++;
                _label.Content = currentTime.ToString();
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
