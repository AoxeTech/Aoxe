using System.Collections.Generic;
using OrderDomain.AggregateRoots;
using Zaaby.Core;

namespace OrderDomain.IRepository
{
    public interface ICurrencyRepository : IZaabyRepository<Currency, string>
    {
        void Add(Currency currency);
        void Add(List<Currency> currencies);
        bool Delete(Currency currency);
        int Delete(List<Currency> currencies);
        bool Modify(Currency currency);
        int Modify(List<Currency> currencies);
        Currency Get(string id);
        List<Currency> Get(List<string> id);
        List<Currency> GetAll();
    }
}