using Core.Entities;

namespace Core.Repositories;

public interface IPlayerRepository
{
    Task<Player> Get(int id);
    Task<IList<Player>> Get(IList<int> ids);
    Task<IList<Player>> List(int bunchId);
    Task<Player> Get(int bunchId, int userId);
    Task<bool> JoinBunch(Player player, Bunch bunch, int userId);
    Task<int> Add(Player player);
    Task Delete(int playerId);
}