using System.Collections.Generic;
using OrderDomain.DomainObjects;
using Zaaby.Core;

namespace IOrderRepository
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