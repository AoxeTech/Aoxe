using System;
using System.Collections.Generic;
using Model;
using Zaaby.ThreeTier.Abstractions.DataAccess;

namespace IDataAccessLayer
{
    public interface IUserDal : IDal
    {
        public int Add(User user);
        public bool Delete(User user);
        public bool Update(User user);
        public User GetById(Guid id);
        public User GetByName(string name);
        public IList<User> GetAll();
    }
}