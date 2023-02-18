using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly PlayerDb _playerDb;
    private readonly ICache _cache;

    public PlayerRepository(IDb db, ICache cache)
    {
        _playerDb = new PlayerDb(db);
        _cache = cache;
    }

    public async Task<Player> Get(string id)
    {
        return await _cache.GetAndStoreAsync(_playerDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Player>> Get(IList<string> ids)
    {
        return await _cache.GetAndStoreAsync(_playerDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
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

    public async Task<string> Add(Player player)
    {
        return await _playerDb.Add(player);
    }

    public async Task<bool> JoinBunch(Player player, Bunch bunch, string userId)
    {
        return await _playerDb.JoinBunch(player, bunch, userId);
    }

    public async Task Delete(string playerId)
    {
        await _playerDb.Delete(playerId);
        _cache.Remove<Player>(playerId);
    }
}