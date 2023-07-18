using System;

namespace EditableTimer
{
    public interface ITimerManager
    {
        /// <summary>
        /// Register an executer, next time execution will be after dueInitialTime.
        /// </summary>
        /// <param name="executer"></param>
        /// <param name="dueInitialTime"></param>
        void RegisterTimer(ITimerExecuter executer, TimeSpan dueInitialTime);
        /// <summary>
        /// Removes from manager the executer, it is expected to be call while dispose or when timer is not required anymore
        /// </summary>
        /// <param name="executer"></param>
        void UnregisterTimer(ITimerExecuter executer);
        /// <summary>
        /// Cancel the current wait time and create a wait based on newTime param
        /// </summary>
        /// <param name="executer"></param>
        /// <param name="newTime"></param>
        void ChangeWaitTime(ITimerExecuter executer, TimeSpan newTime);
    }
}
