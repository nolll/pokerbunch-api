using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class LocationDb(IDb db)
{
    private static Query LocationQuery => new(Schema.Location);

    private static Query GetQuery => LocationQuery
        .Select(
            Schema.Location.Id,
            Schema.Location.Name,
            Schema.Location.BunchId)
        .SelectRaw($"{Schema.Bunch.Name} AS {Schema.Bunch.Slug.AsParam()}")
        .LeftJoin(Schema.Bunch, Schema.Bunch.Id, Schema.Location.BunchId);

    private static Query FindQuery => LocationQuery
        .Select(Schema.Location.Id)
        .LeftJoin(Schema.Bunch, Schema.Bunch.Id, Schema.Location.BunchId);

    public async Task<Location> Get(string id)
    {
        var query = GetQuery.Where(Schema.Location.Id, int.Parse(id));
        var locationDto = await db.FirstOrDefaultAsync<LocationDto>(query);
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
        var locationDtos = await db.GetAsync<LocationDto>(query);
        return locationDtos.Select(LocationMapper.ToLocation).ToList();
    }

    public async Task<IList<string>> Find(string slug)
    {
        var query = FindQuery.Where(Schema.Bunch.Name, slug);
        return (await db.GetAsync<int>(query)).Select(o => o.ToString()).ToList();
    }
        
    public async Task<string> Add(Location location)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.Location.Name, location.Name },
            { Schema.Location.BunchId, int.Parse(location.BunchId) }
        };

        var result = await db.InsertGetIdAsync(LocationQuery, parameters);
        return result.ToString();
    }
}