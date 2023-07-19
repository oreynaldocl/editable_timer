using System;
using System.Threading.Tasks;

namespace EditableTimer.Interfaces
{
    public interface ITaskFactory
    {
        Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state);
    }
}
