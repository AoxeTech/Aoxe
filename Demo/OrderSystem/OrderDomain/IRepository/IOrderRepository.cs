using System;
using System.Collections.Generic;
using OrderDomain.AggregateRoots;
using Zaaby.Core;

namespace OrderDomain.IRepository
{
    public interface IOrderRepository : IZaabyRepository<Order, string>
    {
        void Add(Order orderParent);
        void Add(List<Order> orderParents);
        bool Delete(Order orderParent);
        int Delete(List<Order> orderParents);
        bool Modify(Order orderParent);
        int Modify(List<Order> orderParents);
        Order Get(string id);
        List<Order> Get(List<string> id);
        List<Order> GetAll();
        List<Order> GetValidOrders();
        List<Order> GetOrdersByUser(Guid userId);
    }
}