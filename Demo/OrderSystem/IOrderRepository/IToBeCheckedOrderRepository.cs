using System.Collections.Generic;
using OrderDomain.DomainObjects;
using Zaaby.Core;

namespace IOrderRepository
{
    public interface IToBeCheckedOrderRepository : IZaabyRepository<ToBeCheckedOrder, string>
    {
        new void Add(ToBeCheckedOrder t);
        new void Add(List<ToBeCheckedOrder> t);
        new bool Delete(ToBeCheckedOrder t);
        new int Delete(List<ToBeCheckedOrder> t);
        new bool Modify(ToBeCheckedOrder t);
        new int Modify(List<ToBeCheckedOrder> ts);
        new ToBeCheckedOrder Get(string id);
        new List<ToBeCheckedOrder> Get(List<string> ids);
        new List<ToBeCheckedOrder> GetAll();
    }
}