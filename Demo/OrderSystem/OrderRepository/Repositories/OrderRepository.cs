using System.Collections.Generic;
using IOrderRepository;
using OrderDomain.DomainObjects;

namespace OrderRepository.Repositories
{
    public class OrderRepository : IOrderParentRepository
    {
        public void Add(Order orderParent)
        {
            throw new System.NotImplementedException();
        }

        public void Add(List<Order> orderParents)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Order orderParent)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(List<Order> orderParents)
        {
            throw new System.NotImplementedException();
        }

        public void Modify(Order orderParent)
        {
            throw new System.NotImplementedException();
        }

        public void Modify(List<Order> orderParents)
        {
            throw new System.NotImplementedException();
        }

        public Order Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public List<Order> Get(List<string> id)
        {
            throw new System.NotImplementedException();
        }
    }
}