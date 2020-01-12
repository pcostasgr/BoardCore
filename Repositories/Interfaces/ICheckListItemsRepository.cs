using BoardCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Repositories.Interfaces
{
    public interface ICheckListItemsRepository
    {
        Task<CheckListItems> AddItem(CheckListItems item);
        Task<CheckListItems> UpdateItem(CheckListItems item);
        Task<Int64> DeleteItem(Int64 itemId);
    }
}