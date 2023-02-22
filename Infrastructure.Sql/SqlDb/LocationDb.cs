using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql.SqlDb;

public class LocationDb
{
    private readonly IDb _db;

    private static Query LocationQuery => new(Schema.Location);

    private static Query GetQuery => LocationQuery
        .Select(
            Schema.Location.Id,
            Schema.Location.Name,
            Schema.Location.BunchId);

    private static Query FindQuery => LocationQuery
        .Select(Schema.Location.Id);

    public LocationDb(IDb db)
    {
        _db = db;
    }

    public async Task<Location> Get(string id)
    {
        var query = GetQuery.Where(Schema.Location.Id, int.Parse(id));
        var locationDto = await _db.QueryFactory.FromQuery(query).FirstOrDefaultAsync<LocationDto>();
        var location = locationDto?.ToLocation();

        if (location is null)
            throw new PokerBunchException($"Location with id {id} was not found");

        return location;
    }
        
    public async Task<IList<Location>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Location>();

        var query = GetQuery.WhereIn(Schema.Location.Id, ids.Select(int.Parse));
        var locationDtos = await _db.GetAsync<LocationDto>(query);
        return locationDtos.Select(LocationMapper.ToLocation).ToList();
    }

    public async Task<IList<string>> Find(string bunchId)
    {
        var query = FindQuery.Where(Schema.Location.BunchId, int.Parse(bunchId));
        return (await _db.GetAsync<int>(query)).Select(o => o.ToString()).ToList();
    }
        
    public async Task<string> Add(Location location)
    {
        var parameters = new Dictionary<string, object>
        {
            { Schema.Location.Name.AsParam(), location.Name },
            { Schema.Location.BunchId.AsParam(), int.Parse(location.BunchId) }
        };

        var result = await _db.QueryFactory.FromQuery(LocationQuery).InsertGetIdAsync<int>(parameters);
        return result.ToString();
    }
}