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

    private static Query TableQuery => new(SqlNames.Location.Table);

    private static Query GetQuery => TableQuery
        .Select(
            SqlNames.Location.Columns.Id,
            SqlNames.Location.Columns.Name, 
            SqlNames.Location.Columns.BunchId);

    private static Query FindQuery => TableQuery
        .Select(SqlNames.Location.Columns.Id);

    public LocationDb(IDb db)
    {
        _db = db;
    }

    public async Task<Location> Get(string id)
    {
        var query = GetQuery.Where(SqlNames.Location.Columns.Id, int.Parse(id));
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

        var query = GetQuery.Where(SqlNames.Location.Columns.Id, ids.Select(int.Parse));
        var locationDtos = await _db.QueryFactory.FromQuery(query).GetAsync<LocationDto>();
        return locationDtos.Select(LocationMapper.ToLocation).ToList();
    }

    public async Task<IList<string>> Find(string bunchId)
    {
        var query = FindQuery.Where(SqlNames.Location.Columns.BunchId, int.Parse(bunchId));
        return (await _db.QueryFactory.FromQuery(query).GetAsync<int>()).Select(o => o.ToString()).ToList();
    }
        
    public async Task<string> Add(Location location)
    {
        var parameters = new Dictionary<string, object>
        {
            { SqlNames.Location.Columns.Name, location.Name },
            { SqlNames.Location.Columns.BunchId, int.Parse(location.BunchId) }
        };

        var result = await _db.QueryFactory.FromQuery(TableQuery).InsertGetIdAsync<int>(parameters);
        return result.ToString();

    }
}