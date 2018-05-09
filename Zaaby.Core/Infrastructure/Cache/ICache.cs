using System;
using System.Collections.Generic;

namespace Zaaby.Core.Infrastructure.Cache
{
    public interface ICache : IDisposable
    {
        void Add<T>(string key, T entity, double expireMins = 0);
        void AddAsync<T>(string key, T entity, double expireMins = 0);
        void AddRange<T>(IList<Tuple<string, T>> entities, double expireMins = 0);
        void AddRangeAsync<T>(IList<Tuple<string, T>> entities, double expireMins = 0);
        void Delete(string key);
        void DeleteAsync(string key);
        void DeleteAll(IList<string> keys);
        void DeleteAllAsync(IList<string> keys);
        T Get<T>(string key);
        Dictionary<string, T> Get<T>(IList<string> keys);
    }
}