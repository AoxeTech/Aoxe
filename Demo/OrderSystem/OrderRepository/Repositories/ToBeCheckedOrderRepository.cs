using System.Collections.Generic;
using OrderDomain.AggregateRoots;
using OrderDomain.IRepository;

namespace OrderRepository.Repositories
{
    public class ToBeCheckedOrderRepository : IToBeCheckedOrderRepository
    {
        public void Add(ToBeCheckedOrder t)
        {
            throw new System.NotImplementedException();
        }

        public void Add(List<ToBeCheckedOrder> t)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(ToBeCheckedOrder t)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(List<ToBeCheckedOrder> t)
        {
            throw new System.NotImplementedException();
        }

        public bool Modify(ToBeCheckedOrder t)
        {
            throw new System.NotImplementedException();
        }

        public int Modify(List<ToBeCheckedOrder> ts)
        {
            throw new System.NotImplementedException();
        }

        public ToBeCheckedOrder Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public List<ToBeCheckedOrder> Get(List<string> ids)
        {
            throw new System.NotImplementedException();
        }

        public List<ToBeCheckedOrder> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}