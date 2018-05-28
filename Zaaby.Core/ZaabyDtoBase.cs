using System;

namespace Zaaby.Core
{
    public class ZaabyDtoBase
    {
        public Guid Id { get; }
        public DateTimeOffset Timespan { get; }
        public string Msg { get; set; }
        public Status Status { get; set; }
        public int ErrCode { get; set; }
        public Guid? LastId { get; set; }
        public object Data { get; set; }

        public ZaabyDtoBase()
        {
            Id = Guid.NewGuid();
            Timespan = DateTimeOffset.Now;
        }
    }

    public enum Status
    {
        Success = 0,
        Failure = 1,
        Warning = 2,
        Info = 3
    }
}