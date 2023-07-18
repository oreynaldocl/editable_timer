using System;
using System.Threading;

namespace EditableTimer
{
    public class TimerItem
    {
        public ITimerExecuter Executer { get; set; }
        public CancellationTokenSource Source { get; set; }
        public DateTime WaitUntil { get; set; }
    }
}
