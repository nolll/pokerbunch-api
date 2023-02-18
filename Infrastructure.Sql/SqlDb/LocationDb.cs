using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;

namespace Infrastructure.Sql.SqlDb;

public class LocationDb
{
    private readonly IDb _db;

    public LocationDb(IDb db)
    {
        _db = db;
    }

    public async Task<Location> Get(string id)
    {
        var @params = new
        {
            id = int.Parse(id)
        };

        var locationDto = await _db.Single<LocationDto>(LocationSql.GetByIdQuery, @params);
        var location = locationDto?.ToLocation();

        if (location is null)
            throw new PokerBunchException($"Location with id {id} was not found");

        return location;
    }
        
    public async Task<IList<Location>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Location>();

        var param = new ListParam("@ids", ids.Select(int.Parse));
        var locationDtos = await _db.List<LocationDto>(LocationSql.GetByIdsQuery, param);
        return locationDtos.Select(LocationMapper.ToLocation).ToList();
    }

    public async Task<IList<string>> Find(string bunchId)
    {
        var @params = new
        {
            bunchId = int.Parse(bunchId)
        };
        
        return (await _db.List<int>(LocationSql.FindByBunch, @params)).Select(o => o.ToString()).ToList();
    }
        
    public async Task<string> Add(Location location)
    {
        var @params = new
        {
            name = location.Name,
            bunchId = int.Parse(location.BunchId)
        };

        return (await _db.Insert(LocationSql.AddQuery, @params)).ToString();
    }
}