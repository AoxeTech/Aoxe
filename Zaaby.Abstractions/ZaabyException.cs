using System;
using System.Runtime.Serialization;

namespace Zaaby.Abstractions
{
    public class ZaabyException : Exception
    {
        public Guid Id { get; } = Guid.NewGuid();

        public ZaabyException()
        {
        }

        public ZaabyException(Guid id, string message) : base(message)
        {
            Id = id;
            Message = message;
        }

        public ZaabyException(Guid id, string message, string stackTrace) : base(message)
        {
            Id = id;
            Message = message;
            StackTrace = stackTrace;
        }

        public ZaabyException(Guid id, string message, Exception inner, string stackTrace) : base(message, inner)
        {
            Id = id;
            Message = message;
            StackTrace = stackTrace;
        }

        protected ZaabyException(
            Guid id,
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            Id = id;
        }

        /// <inheritdoc />
        /// <summary>Gets a message that describes the current exception.</summary>
        /// <returns>The error message that explains the reason for the exception, or an empty string ("").</returns>
        public override string Message { get; }

        /// <inheritdoc />
        /// <summary>Gets or sets the name of the application or the object that causes the error.</summary>
        /// <returns>The name of the application or the object that causes the error.</returns>
        /// <exception cref="T:System.ArgumentException">The object must be a runtime <see cref="N:System.Reflection"></see> object</exception>
        public override string Source { get; set; }

        /// <inheritdoc />
        /// <summary>Gets a string representation of the immediate frames on the call stack.</summary>
        /// <returns>A string that describes the immediate frames of the call stack.</returns>
        public override string StackTrace { get; }
    }
}