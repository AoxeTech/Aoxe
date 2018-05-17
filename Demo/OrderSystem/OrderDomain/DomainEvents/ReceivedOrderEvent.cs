using Zaaby.Core.Domain;

namespace OrderDomain.DomainEvents
{
    public class ReceivedOrderEvent : DomainEvent
    {
        public string OrderId { get; set; }
        public int ChargeLengthByMillimeter { get; set; }
        public int ChargeWidthByMillimeter { get; set; }
        public int ChargeHeightByMillimeter { get; set; }
        public int ChargeWeightByGram { get; set; }
    }
}