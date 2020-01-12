using BoardCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Repositories.Interfaces
{
    public interface IListsRepository
    {
        Task<Lists> GetByID(Int64 listId);
        Task<List<Lists>> GetByUserId(Int64 userId);
        Task<List<Lists>> GetAll();
        Task<Lists> AddList(Lists list);
        Task<Lists> UpdateList(Lists list);
        Task<Int64> DeleteList(Int64 listid);
    }
}