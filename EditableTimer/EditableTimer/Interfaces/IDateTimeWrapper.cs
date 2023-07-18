using System;

namespace EditableTimer
{
    public interface IDateTimeWrapper
    {
        DateTime UtcNow { get; }
    }
}
