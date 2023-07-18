using System;

namespace EditableTimer.Extensions
{
    public static class PeriodExtensions
    {
        public static void CheckPositive(this TimeSpan current, string errorMessage = null)
        {
            if (current.TotalMilliseconds < 0)
            {
                string message = errorMessage != null ? errorMessage : $"Not possible to manage negative periods: {current.TotalMilliseconds}ms";
                throw new Exception(message);
            }
        }

    }
}
