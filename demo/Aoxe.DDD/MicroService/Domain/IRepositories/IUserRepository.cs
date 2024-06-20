using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aoxe.DDD.Abstractions.Infrastructure.Repository;
using Domain.AggregateRoots;

namespace Domain.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task AddAsync(User user);
        Task<User> GetAsync(Guid id);
        Task<IList<User>> GetAllAsync();
    }
}
