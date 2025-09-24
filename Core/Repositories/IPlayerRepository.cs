using Core.Entities;

namespace Core.Repositories;

public interface IPlayerRepository
{
    Task<Player> Get(string id);
    Task<IList<Player>> Get(IList<string> ids);
    Task<IList<Player>> List(string slug);
    Task<Player?> Get(string bunchId, string userId);
    Task<bool> JoinBunch(Player player, Bunch bunch, string userId);
    Task<string> Add(Player player);
    Task Delete(string playerId);
}