using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class BunchRepository : IBunchRepository
{
    private readonly SqlBunchDb _bunchDb;
    private readonly ICacheContainer _cacheContainer;

    public BunchRepository(PostgresDb db, ICacheContainer cacheContainer)
    {
        _bunchDb = new SqlBunchDb(db);
        _cacheContainer = cacheContainer;
    }

    public async Task<Bunch> Get(string id)
    {
        return await _cacheContainer.GetAndStoreAsync(_bunchDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<Bunch> GetBySlug(string slug)
    {
        var ids = await Search(slug);
        if (ids.Any())
            return await Get(ids.First());
        return null;
    }

    private async Task<IList<Bunch>> List(IList<string> ids)
    {
        return await _cacheContainer.GetAndStoreAsync(_bunchDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Bunch>> List()
    {
        var ids = await _bunchDb.Search();
        return await List(ids);
    }

    private async Task<IList<string>> Search(string slug)
    {
        return await _bunchDb.Search(slug);
    }

    public async Task<IList<Bunch>> List(string userId)
    {
        var ids = await _bunchDb.SearchByUser(userId);
        return await List(ids);
    }

    public async Task<string> Add(Bunch bunch)
    {
        return await _bunchDb.Add(bunch);
    }

    public async Task Update(Bunch bunch)
    {
        await _bunchDb.Update(bunch);
        _cacheContainer.Remove<Bunch>(bunch.Id);
    }
}