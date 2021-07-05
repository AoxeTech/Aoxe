using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.AggregateRoots;
using Domain.DomainServices;
using Domain.Entities;
using Domain.ValueObjects;
using Zaaby.DDD.Abstractions.Application;

namespace Application
{
    public class UserApplicationService : IApplicationService
    {
        private readonly UserDomainService _userDomainService;

        public UserApplicationService(UserDomainService userDomainService)
        {
            _userDomainService = userDomainService;
        }

        public async Task<IList<User>> GetAllUsersAsync()
        {
            return await _userDomainService.GetAllUsersAsync();
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _userDomainService.GetUserAsync(userId);
        }
        
        public async Task AddUser()
        {
            //注意:不要使用自增字段，使用自增字段相当于要先持久化才能获取到Id，实际上是破坏了聚合的完整性。
            var user = new User(Guid.NewGuid(), "This is user name",
                DateTime.UtcNow.Hour, Gender.Female, "CN", "GuangDong", "GuangZhou", "YueXiu");
            await _userDomainService.AddUser(user);
        }

        public async Task ChangeNameAsync(Guid userId, string name)
        {
            await _userDomainService.ChangeNameAsync(userId, name);
        }

        public async Task CelebrateBirthdayAsync(Guid userId)
        {
            await _userDomainService.CelebrateBirthdayAsync(userId);
        }
        
        public async Task SetTagsAsync(Guid userId, IEnumerable<string> tags)
        {
            await _userDomainService.SetTagsAsync(userId, tags.ToArray());
        }

        public async Task AddCardAsync(Guid userId, string cardName)
        {
            await _userDomainService.AddCardAsync(userId, new Card(Guid.NewGuid(), cardName));
        }
    }
}