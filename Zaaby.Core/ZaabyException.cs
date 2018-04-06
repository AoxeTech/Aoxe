using System;
using System.Runtime.Serialization;

namespace Zaaby.Core
{
    public class ZaabyException : Exception
    {
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