using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class PlayerRepository(IDb db, ICache cache) : IPlayerRepository
{
    private readonly PlayerDb _playerDb = new(db);

    public Task<Player> Get(string id)
    {
        return cache.GetAndStoreAsync(_playerDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public Task<IList<Player>> Get(IList<string> ids)
    {
        return cache.GetAndStoreAsync(_playerDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Player>> List(string bunchId)
    {
        var ids = await _playerDb.Find(bunchId);
        return await Get(ids);
    }

    public async Task<Player?> Get(string bunchId, string userId)
    {
        var ids = await _playerDb.FindByUser(bunchId, userId);
        if (!ids.Any())
            return null;
        return (await Get(ids)).First();
    }

    public Task<string> Add(Player player)
    {
        return _playerDb.Add(player);
    }

    public Task<bool> JoinBunch(Player player, Bunch bunch, string userId)
    {
        return _playerDb.JoinBunch(player, bunch, userId);
    }

    public async Task Delete(string playerId)
    {
        await _playerDb.Delete(playerId);
        cache.Remove<Player>(playerId);
    }
}