using System.Collections.Generic;
using OrderDomain.DomainObjects;
using Zaaby.Core;

namespace IOrderRepository
{
    public interface IOrderParentRepository : IZaabyRepository<Order, string>
    {
        new void Add(Order orderParent);
        new void Add(List<Order> orderParents);
        new bool Delete(Order orderParent);
        new int Delete(List<Order> orderParents);
        new bool Modify(Order orderParent);
        new int Modify(List<Order> orderParents);
        new Order Get(string id);
        new List<Order> Get(List<string> id);
        new List<Order> GetAll();
    }
}