using System;
using OrderDomain.ValueObjects;
using Zaaby.Core;

namespace OrderDomain.Entities
{
    public class OrderBase : IEntity<string>
    {
        public string Id { get; protected set; }
        public DateTimeOffset CreateTimeOffset { get; protected set; }
        public string CustomerId { get; protected set; }
        public int Lenght { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Weight { get; protected set; }
        public OrderStatus Status { get; protected set; }
    }
}