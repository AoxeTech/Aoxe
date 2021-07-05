using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.AggregateRoots;
using Domain.Entities;
using Domain.IRepositories;
using Zaaby.DDD.Abstractions.Domain;

namespace Domain.DomainServices
{
    public class UserDomainService : IDomainService
    {
        private readonly IUserRepository _userRepository;

        public UserDomainService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IList<User>> GetAllUsersAsync() => await _userRepository.GetAllAsync();

        public async Task<User> GetUserAsync(Guid userId) => await _userRepository.GetAsync(userId);

        public async Task AddUser(User user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task ChangeNameAsync(Guid userId, string name)
        {
            var user = await _userRepository.GetAsync(userId);
            user?.ChangeName(name);
        }

        public async Task CelebrateBirthdayAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            user?.CelebrateBirthday();
        }

        public async Task SetTagsAsync(Guid userId, params string[] tags)
        {
            var user = await _userRepository.GetAsync(userId);
            user?.SetTags(tags);
        }

        public async Task AddCardAsync(Guid userId, Card card)
        {
            var user = await _userRepository.GetAsync(userId);
            user?.AddCard(card);
        }
    }
}