using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BoardCore.Repositories;
using BoardCore.Models;
using BoardCore.Helpers;
using System.Threading.Tasks;

namespace BoardCore.Services
{
    public interface IUserService
    {
        Task<Users> Authenticate(string username, string password);
        IEnumerable<Users> GetAll();


    }

    public class UserDbService:IUserService{
        private readonly AppSettings _appSettings;
        public UserDbService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        async public Task<Users> Authenticate(string username, string password)
        {

            var userRepo=new UsersRepository(_appSettings.ConnectionString);

            var user=await userRepo.GetUser(username,password);

            // return null if user not found
            if (user == null)
                return null;

            user.Token=TokenCreator.CreateToken(user.UserId,_appSettings.Secret);

            return user.WithoutPassword();
        }

        public IEnumerable<Users> GetAll()
        {
            return null;
        }

        
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Users> _users = new List<Users>
        { 
            new Users { UserId = 1, FirstName = "Test", LastName = "EUser", Username = "test", Password = "test" } 
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        async public Task<Users> Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;


            user.Token=TokenCreator.CreateToken(user.UserId,_appSettings.Secret);

            return await Task.FromResult(user.WithoutPassword());
        }

        public IEnumerable<Users> GetAll()
        {
            return _users.WithoutPasswords();
        }

    }
}