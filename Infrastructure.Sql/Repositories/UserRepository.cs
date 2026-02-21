using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Models;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class UserRepository(PokerBunchDbContext db, ICache cache) : IUserRepository
{
    private readonly UserDb _userDb = new(db);

    public Task<User> GetById(string id) => GetAndCache(id);

    public async Task<IList<User>> List()
    {
        var ids = await _userDb.Find();
        return await GetAndCache(ids);
    }

    public async Task<User> GetByUserEmail(string email)
    {
        var id = await _userDb.FindByEmail(email.ToLower());
        return id != null 
            ? await GetAndCache(id) 
            : throw new PokerBunchException($"User not found: {email}");
    }

    public async Task<User> GetByUserNameOrEmail(string nameOrEmail)
    {
        var id = await _userDb.FindByUserNameOrEmail(nameOrEmail.ToLower());
        return id != null 
            ? await GetAndCache(id) 
            : throw new PokerBunchException($"User not found: {nameOrEmail}");
    }

    public async Task<User> GetByUserName(string name)
    {
        var id = await _userDb.FindByUserName(name.ToLower());
        return id != null 
            ? await GetAndCache(id) 
            : throw new PokerBunchException($"User not found: {name}");
    }

    public async Task Update(User user)
    {
        await _userDb.Update(user);
        cache.Remove<User>(user.Id);
    }

    public Task<string> Add(User user) => _userDb.Add(user);

    private Task<User> GetAndCache(string id) => 
        cache.GetAndStoreAsync(_userDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));

    private Task<IList<User>> GetAndCache(IList<string> ids) => 
        cache.GetAndStoreAsync(_userDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
}