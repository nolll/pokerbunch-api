using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDb _userDb;
    private readonly ICache _cache;

    public UserRepository(IDb db, ICache cache)
    {
        _userDb = new UserDb(db);
        _cache = cache;
    }

    public async Task<User> GetById(string id)
    {
        return await GetAndCache(id);
    }

    public async Task<IList<User>> List()
    {
        var ids = await _userDb.Find();
        return await GetAndCache(ids);
    }

    public async Task<User> GetByUserEmail(string email)
    {
        var id = await _userDb.FindByEmail(email.ToLower());
        if (id == null)
            throw new PokerBunchException($"User not found: {email}");

        return await GetAndCache(id);
    }

    public async Task<User> GetByUserNameOrEmail(string nameOrEmail)
    {
        var id = await _userDb.FindByUserNameOrEmail(nameOrEmail.ToLower());
        if (id == null)
            throw new PokerBunchException($"User not found: {nameOrEmail}");

        return await GetAndCache(id);
    }

    public async Task<User> GetByUserName(string name)
    {
        var id = await _userDb.FindByUserName(name.ToLower());
        if (id == null)
            throw new PokerBunchException($"User not found: {name}");

        return await GetAndCache(id);
    }

    public async Task Update(User user)
    {
        await _userDb.Update(user);
        _cache.Remove<User>(user.Id);
    }

    public async Task<string> Add(User user)
    {
        return await _userDb.Add(user);
    }

    private async Task<User> GetAndCache(string id)
    {
        return await _cache.GetAndStoreAsync(_userDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private async Task<IList<User>> GetAndCache(IList<string> ids)
    {
        return await _cache.GetAndStoreAsync(_userDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }
}