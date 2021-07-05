using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.AggregateRoots;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CustomDbContext _dbContext;

        public UserRepository(CustomDbContext customDbContext)
        {
            _dbContext = customDbContext;
        }
        
        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<IList<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}