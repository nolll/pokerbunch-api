using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Infrastructure.Sql.SqlDb;

namespace Infrastructure.Sql.Repositories;

public class BunchRepository : IBunchRepository
{
    private readonly SqlBunchDb _bunchDb;
    private readonly ICacheContainer _cacheContainer;

    public BunchRepository(PostgresStorageProvider db, ICacheContainer cacheContainer)
    {
        _bunchDb = new SqlBunchDb(db);
        _cacheContainer = cacheContainer;
    }

    public Bunch Get(int id)
    {
        return _cacheContainer.GetAndStore(_bunchDb.Get, id, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public Bunch GetBySlug(string slug)
    {
        var ids = Search(slug);
        if (ids.Any())
            return Get(ids.First());
        return null;
    }

    private IList<Bunch> List(IList<int> ids)
    {
        return _cacheContainer.GetAndStore(_bunchDb.Get, ids, TimeSpan.FromMinutes(CacheTime.Long));
    }

    public IList<Bunch> List()
    {
        var ids = _bunchDb.Search();
        return List(ids);
    }

    private IList<int> Search(string slug)
    {
        return _bunchDb.Search(slug);
    }

    public IList<Bunch> List(int userId)
    {
        var ids = _bunchDb.Search(userId);
        return List(ids);
    }

    public async Task<int> Add(Bunch bunch)
    {
        return await _bunchDb.Add(bunch);
    }

    public async Task Update(Bunch bunch)
    {
        await _bunchDb.Update(bunch);
        _cacheContainer.Remove<Bunch>(bunch.Id);
    }
}