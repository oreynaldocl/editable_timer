using EditableTimer.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace EditableTimer
{
    [ExcludeFromCodeCoverage]
    public class TaskFactoryWrapper : ITaskFactory
    {
        public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state)
        {
            return Task.Factory.StartNew(function, state);
        }
    }
}
