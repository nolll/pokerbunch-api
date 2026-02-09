using System.Linq;
using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.Models;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class BunchRepository(IDb db, ICache cache, PokerBunchDbContext dbContext) : IBunchRepository
{
    private readonly BunchDb _bunchDb = new(db, dbContext);

    public Task<Bunch> Get(string id)
    {
        return cache.GetAndStoreAsync(_bunchDb.Get2, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<Bunch> GetBySlug(string slug)
    {
        var ids = await Search(slug);
        if (!ids.Any())
            throw new PokerBunchException($"Bunch with slug {slug} was not found");
        
        return await Get(ids.First());
    }

    public async Task<Bunch?> GetBySlugOrNull(string slug)
    {
        var ids = await Search(slug);
        if (ids.Any())
            return await Get(ids.First());

        return null;
    }

    private Task<IList<Bunch>> List(IList<string> ids)
    {
        return cache.GetAndStoreAsync(_bunchDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public async Task<IList<Bunch>> List()
    {
        var ids = await _bunchDb.Search();
        return await List(ids);
    }

    private Task<IList<string>> Search(string slug)
    {
        return _bunchDb.Search(slug);
    }

    public async Task<IList<Bunch>> List(string userId)
    {
        var ids = await _bunchDb.SearchByUser(userId);
        return await List(ids);
    }

    public Task<string> Add(Bunch bunch)
    {
        return _bunchDb.Add(bunch);
    }

    public async Task Update(Bunch bunch)
    {
        await _bunchDb.Update(bunch);
        cache.Remove<Bunch>(bunch.Id);
    }
}