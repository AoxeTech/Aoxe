using System.Collections.Generic;
using OrderDomain.AggregateRoots;
using Zaaby.Core;

namespace OrderDomain.IRepository
{
    public interface IToBeCheckedOrderRepository : IZaabyRepository
    {
        void Add(ToBeCheckedOrder t);
        void Add(List<ToBeCheckedOrder> t);
        bool Delete(ToBeCheckedOrder t);
        int Delete(List<ToBeCheckedOrder> t);
        bool Modify(ToBeCheckedOrder t);
        int Modify(List<ToBeCheckedOrder> ts);
        ToBeCheckedOrder Get(string id);
        List<ToBeCheckedOrder> Get(List<string> ids);
        List<ToBeCheckedOrder> GetAll();
    }
}