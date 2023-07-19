using System.Diagnostics.CodeAnalysis;

namespace EditableTimer
{
    [ExcludeFromCodeCoverage]
    internal class DefaultLogger : ILogger
    {
        public void Log(string message)
        { }
    }
}
