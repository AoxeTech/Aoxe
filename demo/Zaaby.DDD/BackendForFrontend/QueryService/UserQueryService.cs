using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using QueryService.Models;

namespace QueryService
{
    public class UserQueryService
    {
        private readonly IDbConnection _dbConnection;

        public UserQueryService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<UserReadModel>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            return await _dbConnection.QueryAsync<UserReadModel>("SELECT * FROM User WHERE Name=@Name",
                new {Name = name});
        }
    }
}