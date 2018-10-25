using System;
using System.Runtime.Serialization;

namespace Zaaby.Abstractions
{
    public class ZaabyException : Exception
    {
        public Guid Id { get; set; }

        public ZaabyException()
        {
        }

        public ZaabyException(string message) : base(message)
        {
        }

        public ZaabyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ZaabyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}