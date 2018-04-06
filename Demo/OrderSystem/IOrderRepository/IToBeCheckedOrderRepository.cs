using System.Collections.Generic;
using OrderDomain.DomainObjects;

namespace IOrderRepository
{
    public interface IToBeCheckedOrderRepository
    {
        void Add(ToBeCheckedOrder toBeCheckedOrder);
        void Add(List<ToBeCheckedOrder> toBeCheckedOrders);
        void Delete(ToBeCheckedOrder toBeCheckedOrder);
        void Delete(List<ToBeCheckedOrder> toBeCheckedOrders);
        void Modify(ToBeCheckedOrder toBeCheckedOrder);
        void Modify(List<ToBeCheckedOrder> toBeCheckedOrders);
        ToBeCheckedOrder Get(string id);
        List<ToBeCheckedOrder> Get(List<string> id);
    }
}