using System;
using System.Collections.Generic;
using System.Data;
using Aoxe.ThreeTier.Abstractions.DataAccess;
using Dapper;
using Model;
using Zaabee.Dapper.Extensions;

namespace DataAccessLayer
{
    public class UserDal : IDal
    {
        private readonly IDbConnection _connection;

        public UserDal(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Add(User user)
        {
            return _connection.Add(user);
        }

        public int Update(User user)
        {
            return _connection.Update(user);
        }

        public int Delete(User user)
        {
            return _connection.DeleteByEntity(user);
        }

        public User GetById(Guid id)
        {
            return _connection.FirstOrDefault<User>(id);
        }

        public User GetByName(string name)
        {
            return _connection.QueryFirstOrDefault<User>(
                "SELECT * FROM \"User\" WHERE \"Name\" = @Name",
                new { Name = name }
            );
        }

        public IList<User> GetAll()
        {
            return _connection.GetAll<User>();
        }
    }
}
