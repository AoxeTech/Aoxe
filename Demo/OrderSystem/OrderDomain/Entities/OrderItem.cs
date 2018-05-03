using System;

namespace OrderDomain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; protected set; }
        public string Sku { get; protected set; }
        public string Color { get; protected set; }
        public decimal UnitPrice { get; protected set; }
        public int Quantity { get; protected set; }
        public decimal TotalPrice { get; private set; }

        public void SetUnitPrice(decimal unitPrice)
        {
            UnitPrice = unitPrice;
            TotalPrice = UnitPrice * Quantity;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
            TotalPrice = UnitPrice * Quantity;
        }
    }
}