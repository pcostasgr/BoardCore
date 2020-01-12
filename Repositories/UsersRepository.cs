using Dapper;
using BoardCore.Models;
using BoardCore.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IConfiguration _config;
        private string connectionString;

        public UsersRepository(IConfiguration config)
        {
            _config = config;
            this.connectionString=_config.GetConnectionString("MyConnectionString");
        }

        public UsersRepository(string connectionString)
        {
            this.connectionString=connectionString;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public async Task<Users> GetUser(string username,string password)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT USERID,USERNAME,PASSWORD,FIRSTNAME,LASTNAME FROM USERS WHERE username = @username and password=@password";
                conn.Open();
                var result = await conn.QueryAsync<Users>(sQuery, new { username=username,password=password});
                return result.FirstOrDefault();
            }
        }
    }
}
