
using BoardCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<Users> GetUser(string username,string password);
 
    }
}