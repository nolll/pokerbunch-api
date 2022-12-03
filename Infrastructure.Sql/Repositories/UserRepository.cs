using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SqlUserDb _userDb;
    private readonly ICacheContainer _cacheContainer;

    public UserRepository(PostgresDb db, ICacheContainer cacheContainer)
    {
        _userDb = new SqlUserDb(db);
        _cacheContainer = cacheContainer;
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
            return null;
        return await GetAndCache(id);
    }

    public async Task<User> GetByUserNameOrEmail(string nameOrEmail)
    {
        var id = await _userDb.FindByNameOrEmail(nameOrEmail.ToLower());
        if (id == null)
            return null;
        return await GetAndCache(id);
    }

    public async Task<User> GetByUserName(string name)
    {
        var id = await _userDb.FindByName(name.ToLower());
        if (id == null)
            return null;
        return await GetAndCache(id);
    }

    public async Task Update(User user)
    {
        await _userDb.Update(user);
        _cacheContainer.Remove<User>(user.Id);
    }

    public async Task<string> Add(User user)
    {
        return await _userDb.Add(user);
    }

    private async Task<User> GetAndCache(string id)
    {
        return await _cacheContainer.GetAndStoreAsync(_userDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    private async Task<IList<User>> GetAndCache(IList<string> ids)
    {
        return await _cacheContainer.GetAndStoreAsync(_userDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }
}