using System;
using Zaaby.Core.Domain;

namespace OrderDomain.AggregateRoots
{
    public class Currency : IAggregateRoot<string>
    {
        public string Id { get; protected set; }
        public string Name { get; protected set; }
        public DateTimeOffset OperateTime { get; protected set; }
        public int OperaterId { get; protected set; }
        public int Sort { get; protected set; }
        public bool IsValid { get; protected set; }
    }
}