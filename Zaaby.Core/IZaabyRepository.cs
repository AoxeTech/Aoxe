using System.Collections.Generic;

namespace Zaaby.Core
{
    public interface IZaabyRepository<T, TId>
    {
        void Add(T t);
        void Add(List<T> t);
        bool Delete(T t);
        int Delete(List<T> t);
        bool Modify(T t);
        int Modify(List<T> ts);
        T Get(TId id);
        List<T> Get(List<TId> ids);
        List<T> GetAll();
    }
}