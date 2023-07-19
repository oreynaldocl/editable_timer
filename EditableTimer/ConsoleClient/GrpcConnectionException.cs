using System;
using System.Runtime.Serialization;

namespace ConsoleClient
{
    [Serializable]
    public class GrpcConnectionException : Exception
    {
        public GrpcConnectionException() { }
        public GrpcConnectionException(string message) : base(message) { }
        public GrpcConnectionException(string message, Exception inner) : base(message, inner) { }
        protected GrpcConnectionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
