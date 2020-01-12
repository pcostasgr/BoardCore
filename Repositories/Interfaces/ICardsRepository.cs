using BoardCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardCore.Repositories.Interfaces
{
    public interface ICardsRepository
    {
        Task<Cards> GetByID(Int64 cardId);
        Task<List<Cards>> GetByListId(Int64 listId);
        Task<Cards> AddCard(Cards card);
        Task<Cards> UpdateCard(Cards card);
        Task<Int64> DeleteCard(Int64 cardid);
    }
}