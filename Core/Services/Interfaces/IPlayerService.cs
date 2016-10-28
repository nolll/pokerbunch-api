using System.Collections.Generic;
using Core.Entities;

namespace Core.Services
{
    public interface IPlayerService
    {
        IList<Player> List(int bunchId);
        IList<Player> Get(IList<int> ids);
        Player Get(int id);
        Player Get(int bunchId, int userId);
        int Add(Player player);
        bool JoinBunch(Player player, Bunch bunch, int userId);
        void Delete(int playerId);
    }
}