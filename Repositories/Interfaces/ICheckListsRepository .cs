using BoardCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Repositories.Interfaces
{
    public interface ICheckListsRepository
    {
        Task<CheckLists> GetByID(Int64 checklistId);
        Task<List<CheckLists>> GetByCardId(Int64 cardId);
        Task<CheckLists> AddCheckList(CheckLists list);
        Task<CheckLists> UpdateCheckList(CheckLists list);
        Task<Int64> DeleteCheckList(Int64 listid);
    }
}