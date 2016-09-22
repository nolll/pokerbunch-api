using System.Collections.Generic;
using Core.Entities;

namespace Core.Services
{
    public interface IPlayerService
    {
        IList<Player> GetList(int bunchId);
        IList<Player> Get(IList<int> ids);
        Player Get(int id);
        Player GetByUserId(int bunchId, int userId);
        int Add(Player player);
        bool JoinHomegame(Player player, Bunch bunch, int userId);
        void Delete(int playerId);
    }
}