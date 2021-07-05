using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Domain.AggregateRoots;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserApplicationService _userApplicationService;

        public UserController(UserApplicationService userApplicationService)
        {
            _userApplicationService = userApplicationService;
        }

        [HttpGet("~/GetAll")]
        public async Task<IList<User>> GetAll()
        {
            return await _userApplicationService.GetAllUsersAsync();
        }

        [HttpGet]
        public async Task<User> Get(Guid id)
        {
            var user = await _userApplicationService.GetUserAsync(id);
            user.AddCard(new Card(Guid.NewGuid(), "Alice"));
            user.CelebrateBirthday();
            user.ChangeName("Bob");
            user.SetTags("Test");
            return user;
        }

        [HttpPost]
        public async Task AddUser()
        {
            await _userApplicationService.AddUser();
        }

        [HttpPut("~/changename")]
        public async Task ChangeNameAsync(Guid userId, [FromBody] string name)
        {
            await _userApplicationService.ChangeNameAsync(userId, name);
        }

        [HttpPut("~/celebratebirthday")]
        public async Task CelebrateBirthdayAsync(Guid userId)
        {
            await _userApplicationService.CelebrateBirthdayAsync(userId);
        }

        [HttpPut("~/settags")]
        public async Task SetTagsAsync(Guid userId, [FromBody] IEnumerable<string> tags)
        {
            await _userApplicationService.SetTagsAsync(userId, tags);
        }

        [HttpPut("~/addcard")]
        public async Task AddCardAsync(Guid userId, [FromBody] string cardName)
        {
            await _userApplicationService.AddCardAsync(userId, cardName);
        }
    }
}