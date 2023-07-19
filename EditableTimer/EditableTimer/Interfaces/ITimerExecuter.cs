using System.Threading.Tasks;
using System;

namespace EditableTimer
{
    /// <summary>
    /// Define the implementations for executing and updating timer.
    /// Every new update of timer needs to create a new Thread.
    /// It is expected that only one thread at time is related with TimerExecuter
    /// </summary>
    public interface ITimerExecuter: IDisposable
    {
        /// <summary>
        /// Unique identifer that will be used for TimerManager
        /// </summary>
        int Identifier { get; }
        /// <summary>
        /// After the current wait time, this method is executed
        /// </summary>
        /// <returns></returns>
        Task ExecuteHandler();
        // TODO Review if we need to call CalculateNextTime after FailureHandler
        /// <summary>
        /// If there is any failure in process this is executed
        /// </summary>
        /// <returns></returns>
        Task FailureHandler();
        // TODO  Review if we need to use TimeSpan / DateTime
        /// <summary>
        /// Returns when will be next execution. Called after ExecuteTask or FailureMethod
        /// </summary>
        /// <returns></returns>
        Task<TimeSpan> CalculateNextTime();
    }
}
