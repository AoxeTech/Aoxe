using System.Collections.Generic;
using OrderDomain.DomainObjects;
using Zaaby.Core;

namespace IOrderRepository
{
    public interface IOrderParentRepository : IZaabyRepository
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
    }
}