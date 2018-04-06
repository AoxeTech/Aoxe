using System.Collections.Generic;
using OrderDomain.DomainObjects;

namespace IOrderRepository
{
    public interface IOrderParentRepository
    {
        void Add(Order orderParent);
        void Add(List<Order> orderParents);
        void Delete(Order orderParent);
        void Delete(List<Order> orderParents);
        void Modify(Order orderParent);
        void Modify(List<Order> orderParents);
        Order Get(string id);
        List<Order> Get(List<string> id);
    }
}