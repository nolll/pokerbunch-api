using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class PlayerRepository(IDb db, ICache cache) : IPlayerRepository
{
    private readonly PlayerDb _playerDb = new(db);

    public Task<Player> Get(string id) =>
        cache.GetAndStoreAsync(_playerDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));

    public Task<IList<Player>> Get(IList<string> ids) =>
        cache.GetAndStoreAsync(_playerDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));

    public async Task<IList<Player>> List(string slug)
    {
        var ids = await _playerDb.Find(slug);
        return await Get(ids);
    }

    public async Task<Player?> Get(string bunchId, string userId)
    {
        var ids = await _playerDb.FindByUser(bunchId, userId);
        return ids.Any()
            ? (await Get(ids)).First()
            : null;
    }

    public Task<string> Add(Player player) => _playerDb.Add(player);

    public Task<bool> JoinBunch(Player player, Bunch bunch, string userId) => _playerDb.JoinBunch(player, bunch, userId);

    public async Task Delete(string playerId)
    {
        await _playerDb.Delete(playerId);
        cache.Remove<Player>(playerId);
    }
}