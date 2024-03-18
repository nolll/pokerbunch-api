using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class UserRepository(IDb db, ICache cache) : IUserRepository
{
    private readonly UserDb _userDb = new(db);

    public Task<User> GetById(string id)
    {
        return GetAndCache(id);
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
        cache.Remove<User>(user.Id);
    }

    public Task<string> Add(User user)
    {
        return _userDb.Add(user);
    }

    private Task<User> GetAndCache(string id)
    {
        return cache.GetAndStoreAsync(_userDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private Task<IList<User>> GetAndCache(IList<string> ids)
    {
        return cache.GetAndStoreAsync(_userDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }
}