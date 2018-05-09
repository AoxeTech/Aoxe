using System;
using System.Collections.Generic;
using System.Linq;
using OrderDomain.Entities;
using OrderDomain.ValueObjects;
using Zaaby.Core.Domain;

namespace OrderDomain.AggregateRoots
{
    public class Order : OrderBase, IAggregateRoot<string>
    {
        public string Id { get; protected set; }
        public string CustomerId { get; protected set; }
        public int LenghtByMillimeter { get; protected set; }
        public int WidthByMillimeter { get; protected set; }
        public int HeightByMillimeter { get; protected set; }
        public int WeightByGram { get; protected set; }
        public OrderStatus Status { get; protected set; }
        public ReceiverAddress ReceiverAddress { get; protected set; }
        public string Currency { get; protected set; }
        public decimal TotalPrice { get; private set; }

        private List<OrderItem> _orderItems;

        public List<OrderItem> OrderItems
        {
            get => _orderItems ?? (_orderItems = new List<OrderItem>());
            protected set => _orderItems = value;
        }

        public Order(string id, string customerId, int lenghtByMillimeter, int widthByMillimeter,
            int heightByMillimeter, int weightByGram, OrderStatus status, ReceiverAddress address, string currency,
            List<OrderItem> items)
        {
            Id = id;
            CustomerId = customerId;
            LenghtByMillimeter = lenghtByMillimeter;
            WidthByMillimeter = widthByMillimeter;
            HeightByMillimeter = heightByMillimeter;
            WeightByGram = weightByGram;
            Status = status;
            ReceiverAddress = address;
            Currency = currency;
            OrderItems = items;
            TotalPrice = OrderItems.Sum(item => item.UnitPrice * item.Quantity);
        }

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

        public void ModifyCurrency(string currency)
        {
            Currency = currency;
        }

        public void ModifyItems(List<OrderItem> items)
        {
            OrderItems = items;
            TotalPrice = OrderItems.Sum(item => item.UnitPrice * item.Quantity);
        }

        public void ModifyVolume(int lenghtByMillimeter, int widthByMillimeter, int heightByMillimeter)
        {
            if (lenghtByMillimeter < 0)
                throw new ArgumentException(nameof(lenghtByMillimeter));
            if (widthByMillimeter < 0)
                throw new ArgumentException(nameof(widthByMillimeter));
            if (heightByMillimeter < 0)
                throw new ArgumentException(nameof(heightByMillimeter));

            LenghtByMillimeter = lenghtByMillimeter;
            WidthByMillimeter = widthByMillimeter;
            HeightByMillimeter = heightByMillimeter;
        }
    }
}