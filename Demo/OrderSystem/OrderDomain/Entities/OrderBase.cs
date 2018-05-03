using System;
using Zaaby.Core;

namespace OrderDomain.Entities
{
    public class OrderBase : IEntity<string>
    {
        public DateTimeOffset CreateTime { get; protected set; }
        public DateTimeOffset LastModifyTime { get; protected set; }
        public DateTimeOffset DeleteTime { get; protected set; }
        public bool IsDelete { get; protected set; }
    }
}