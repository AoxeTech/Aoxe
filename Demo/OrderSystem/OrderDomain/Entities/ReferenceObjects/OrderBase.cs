using System;

namespace OrderDomain.Entities.ReferenceObjects
{
    public class OrderBase
    {
        public string Id { get; set; }
        public DateTimeOffset CreateTimeOffset { get; set; }
    }
}