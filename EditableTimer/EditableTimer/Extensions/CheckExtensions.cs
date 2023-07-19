using System;

namespace EditableTimer.Extensions
{
    public static class CheckExtensions
    {
        public static void CheckNull<TException>(this object objToVerify, string logMsg) where TException : Exception
        {
            if (objToVerify == null)
            {
                var args = new object[] { logMsg };
                var exception = (TException)Activator.CreateInstance(typeof(TException), args);
                throw exception;
            }
        }
    }
}
