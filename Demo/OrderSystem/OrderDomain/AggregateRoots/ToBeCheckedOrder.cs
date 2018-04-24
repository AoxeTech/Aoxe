using OrderDomain.Entities;

namespace OrderDomain.AggregateRoots
{
    public class ToBeCheckedOrder : OrderBase
    {
        public Order Commit()
        {
            return new Order();
        }
    }
}