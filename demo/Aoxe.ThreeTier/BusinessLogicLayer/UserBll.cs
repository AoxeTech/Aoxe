using System;
using System.Collections.Generic;
using Aoxe.SequentialGuid;
using Aoxe.ThreeTier.Abstractions.BusinessLogic;
using BusinessLogicLayer.Dtos;
using DataAccessLayer;
using Model;

namespace BusinessLogicLayer
{
    public class UserBll : IBll
    {
        private readonly UserDal _userDal;

        public UserBll(UserDal userDal)
        {
            _userDal = userDal;
        }

        public bool Login(LoginRequestParam param)
        {
            var user = _userDal.GetByName(param.Name);
            if (user is null)
                throw new ApplicationException("The user is not exist.");
            return user.Pwd == param.Pwd;
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
