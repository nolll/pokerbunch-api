using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly SqlPlayerDb _playerDb;
    private readonly ICacheContainer _cacheContainer;

    public PlayerRepository(PostgresStorageProvider db, ICacheContainer cacheContainer)
    {
        _playerDb = new SqlPlayerDb(db);
        _cacheContainer = cacheContainer;
    }

    public async Task<Player> Get(int id)
    {
        return await _cacheContainer.GetAndStoreAsync(_playerDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Player>> Get(IList<int> ids)
    {
        return await _cacheContainer.GetAndStoreAsync(_playerDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Player>> List(int bunchId)
    {
        var ids = await _playerDb.Find(bunchId);
        return await Get(ids);
    }

    public async Task<Player> Get(int bunchId, int userId)
    {
        var ids = await _playerDb.Find(bunchId, userId);
        if (!ids.Any())
            return null;
        return (await Get(ids)).First();

    }

    public async Task<int> Add(Player player)
    {
        return await _playerDb.Add(player);
    }

    public async Task<bool> JoinBunch(Player player, Bunch bunch, int userId)
    {
        return await _playerDb.JoinBunch(player, bunch, userId);
    }

    public async Task Delete(int playerId)
    {
        await _playerDb.Delete(playerId);
        _cacheContainer.Remove<Player>(playerId);
    }
}