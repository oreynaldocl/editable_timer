using System;
using System.Diagnostics.CodeAnalysis;

namespace EditableTimer
{
    [ExcludeFromCodeCoverage]
    public class DateTimeWrapper: IDateTimeWrapper
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
