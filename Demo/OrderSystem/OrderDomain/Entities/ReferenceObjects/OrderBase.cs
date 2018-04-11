using System;

namespace OrderDomain.Entities.ReferenceObjects
{
    public class OrderBase
    {
        public string Id { get; protected set; }
        public DateTimeOffset CreateTimeOffset { get; protected set; }
        public int UserId { get; protected set; }
        public int Lenght { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Weight { get; protected set; }
    }
}