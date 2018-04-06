using System.Collections.Generic;
using IOrderRepository;
using OrderDomain.DomainObjects;

namespace OrderRepository.Repositories
{
    public class ToBeCheckedOrderRepository : IToBeCheckedOrderRepository
    {
        public void Add(ToBeCheckedOrder toBeCheckedOrder)
        {
            throw new System.NotImplementedException();
        }

        public void Add(List<ToBeCheckedOrder> toBeCheckedOrders)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(ToBeCheckedOrder toBeCheckedOrder)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(List<ToBeCheckedOrder> toBeCheckedOrders)
        {
            throw new System.NotImplementedException();
        }

        public void Modify(ToBeCheckedOrder toBeCheckedOrder)
        {
            throw new System.NotImplementedException();
        }

        public void Modify(List<ToBeCheckedOrder> toBeCheckedOrders)
        {
            throw new System.NotImplementedException();
        }

        public ToBeCheckedOrder Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public List<ToBeCheckedOrder> Get(List<string> id)
        {
            throw new System.NotImplementedException();
        }
    }
}