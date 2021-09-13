using System;

namespace Zaaby.Shared
{
    public class ZaabyError
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTimeOffset ThrowTime { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }
}