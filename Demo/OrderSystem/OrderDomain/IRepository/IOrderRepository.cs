using System;
using System.Collections.Generic;
using OrderDomain.AggregateRoots;
using Zaaby.Core.Infrastructure.Repository;

namespace OrderDomain.IRepository
{
    public interface IOrderRepository : IRepository<Order, string>
    {
        void Add(Order order);
        void Add(List<Order> orders);
        bool Delete(Order order);
        int Delete(List<Order> orders);
        bool Modify(Order order);
        int Modify(List<Order> orders);
        Order Get(string id);
        List<Order> Get(List<string> id);
        List<Order> GetAll();
        List<Order> GetValidOrders();
        List<Order> GetOrdersByUser(Guid userId);
    }
}