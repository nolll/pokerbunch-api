using System.Linq;
using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class BunchRepository(IDb db, ICache cache) : IBunchRepository
{
    private readonly BunchDb _bunchDb = new(db);

    public Task<Bunch> Get(string id) => 
        cache.GetAndStoreAsync(_bunchDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));

    public async Task<Bunch> GetBySlug(string slug)
    {
        var ids = await Search(slug);
        return ids.Any() 
            ? await Get(ids.First()) 
            : throw new PokerBunchException($"Bunch with slug {slug} was not found");
    }

    public async Task<Bunch?> GetBySlugOrNull(string slug)
    {
        var ids = await Search(slug);
        return ids.Any()
            ? await Get(ids.First()) 
            : null;
    }

    private Task<IList<Bunch>> List(IList<string> ids) => 
        cache.GetAndStoreAsync(_bunchDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));

    public async Task<IList<Bunch>> List()
    {
        var ids = await _bunchDb.Search();
        return await List(ids);
    }

    private Task<IList<string>> Search(string slug) => _bunchDb.Search(slug);

    public async Task<IList<Bunch>> List(string userId)
    {
        var ids = await _bunchDb.SearchByUser(userId);
        return await List(ids);
    }

    public Task<string> Add(Bunch bunch) => _bunchDb.Add(bunch);

    public async Task Update(Bunch bunch)
    {
        await _bunchDb.Update(bunch);
        cache.Remove<Bunch>(bunch.Id);
    }
}