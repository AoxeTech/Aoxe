using System;

namespace Zaaby.Core
{
    public class ZaabyDtoBase<T>
    {
        public Guid Id { get; set; }
        public DateTimeOffset Timespan { get; set; }
        public string Msg { get; set; }
        public Status Status { get; set; }
        public int ErrCode { get; set; }
        public T Data { get; set; }
    }

    public enum Status
    {
        Success = 0,
        Failure = 1,
        Warning = 2,
        Info = 3
    }
}