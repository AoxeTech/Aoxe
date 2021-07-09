using System;
using System.Collections.Generic;
using IBusinessLogicLayer;
using IDataAccessLayer;
using Model;
using Zaabee.SequentialGuid;
using Zaaby.ThreeTier.Abstractions.BusinessLogic;

namespace BusinessLogicLayer
{
    public class UserBll : IBll
    {
        private readonly IUserDal _userDal;

        public UserBll(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public bool Login(string name, string pwd)
        {
            var user = _userDal.GetByName(name);
            if (user is null) throw new ApplicationException("The user is not exist.");
            return user.Pwd == pwd;
        }

        public bool CreateUser(CreateUserDto dto)
        {
            var user = new User
            {
                Id = SequentialGuidHelper.GenerateComb(),
                Name = dto.Name,
                Pwd = dto.Pwd,
                Nickname = dto.Nickname,
                CreatedUtcTime = DateTime.UtcNow
            };
            return _userDal.Add(user) > 0;
        }

        public User GetById(Guid id)
        {
            return _userDal.GetById(id);
        }

        public IList<User> GetAll()
        {
            return _userDal.GetAll();
        }
    }
}