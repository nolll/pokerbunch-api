using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
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

        var rawLocation = await _db.Single<RawLocation>(LocationSql.GetByIdQuery, @params);
        return rawLocation != null
            ? CreateLocation(rawLocation)
            : null;
    }
        
    public async Task<IList<Location>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Location>();

        var param = new ListParam("@ids", ids.Select(int.Parse));
        var rawLocations = await _db.List<RawLocation>(LocationSql.GetByIdsQuery, param);
        return rawLocations.Select(CreateLocation).ToList();
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
    
    private Location CreateLocation(RawLocation rawLocation)
    {
        return new Location(
            rawLocation.Location_Id,
            rawLocation.Name,
            rawLocation.Bunch_Id);
    }
}