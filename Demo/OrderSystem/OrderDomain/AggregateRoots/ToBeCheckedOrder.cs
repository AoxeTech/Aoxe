using OrderDomain.Entities.ReferenceObjects;

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