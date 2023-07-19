using System;
using System.Diagnostics.CodeAnalysis;

namespace EditableTimer
{
    [ExcludeFromCodeCoverage]
    internal class DefaultLogger : ILogger
    {
        public void Log(string message)
        { }

        public void LogError(string message, Exception ex)
        { }
    }
}
