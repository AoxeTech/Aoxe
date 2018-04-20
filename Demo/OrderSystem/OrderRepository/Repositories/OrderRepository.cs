using System.Collections.Generic;
using OrderDomain.AggregateRoots;
using OrderDomain.IRepository;
using OrderRepository.PersistentObjects;

namespace OrderRepository.Repositories
{
    public class OrderRepository : IOrderParentRepository
    {
        public void Add(Order orderParent)
        {
            var orderParentPo = new OrderParentPo { };
        }

        public void Add(List<Order> orderParents)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Order orderParent)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(List<Order> orderParents)
        {
            throw new System.NotImplementedException();
        }

        public bool Modify(Order orderParent)
        {
            throw new System.NotImplementedException();
        }

        public int Modify(List<Order> orderParents)
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

        public List<Order> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}