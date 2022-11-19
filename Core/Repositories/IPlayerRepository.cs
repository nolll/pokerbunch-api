using Core.Entities;

namespace Core.Repositories;

public interface IPlayerRepository
{
    Player Get(int id);
    IList<Player> Get(IList<int> ids);

    IList<Player> List(int bunchId);
    Player Get(int bunchId, int userId);

    bool JoinBunch(Player player, Bunch bunch, int userId);
    int Add(Player player);
    void Delete(int playerId);
}