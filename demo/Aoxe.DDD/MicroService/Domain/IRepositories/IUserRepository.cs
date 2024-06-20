using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.AggregateRoots;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace Domain.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task AddAsync(User user);
        Task<User> GetAsync(Guid id);
        Task<IList<User>> GetAllAsync();
    }
}