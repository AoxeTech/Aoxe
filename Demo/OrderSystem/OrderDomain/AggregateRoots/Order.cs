using System;
using OrderDomain.Entities;
using OrderDomain.ValueObjects;

namespace OrderDomain.AggregateRoots
{
    public class Order : OrderBase
    {
        public void Committing()
        {
            if (Status != OrderStatus.ToBeChecked)
                throw new Exception("The order status must be to be checked.");
            Status = OrderStatus.Commited;
        }

        public void Receiving()
        {
            if (Status != OrderStatus.Commited)
                throw new Exception("The order status must be commited.");
            Status = OrderStatus.Received;
        }
    }
}