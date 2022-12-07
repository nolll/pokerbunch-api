using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.SqlDb;

public class SqlLocationDb
{
    private const string DataSql = @"
        SELECT l.location_id, l.name, l.bunch_id
        FROM pb_location l ";

    private const string SearchIdSql = @"
        SELECT l.location_id
        FROM pb_location l ";

    private readonly IDb _db;

    public SqlLocationDb(IDb db)
    {
        _db = db;
    }

    public async Task<Location> Get(string id)
    {
        var sql = string.Concat(DataSql, "WHERE l.location_id = @id");

        var @params = new
        {
            id = int.Parse(id)
        };

        var rawLocation = await _db.Single<RawLocation>(sql, @params);
        return rawLocation != null
            ? CreateLocation(rawLocation)
            : null;
    }
        
    public async Task<IList<Location>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Location>();

        var sql = string.Concat(DataSql, "WHERE l.location_id IN (@ids)");
        var param = new ListParam("@ids", ids.Select(int.Parse));
        var rawLocations = await _db.List<RawLocation>(sql, param);
        return rawLocations.Select(CreateLocation).ToList();
    }

    public async Task<IList<string>> Find(string bunchId)
    {
        var sql = string.Concat(SearchIdSql, "WHERE l.bunch_id = @bunchId");
        
        var @params = new
        {
            bunchId = int.Parse(bunchId)
        };
        
        return (await _db.List<int>(sql, @params)).Select(o => o.ToString()).ToList();
    }
        
    public async Task<string> Add(Location location)
    {
        const string sql = @"
            INSERT INTO pb_location (name, bunch_id)
            VALUES (@name, @bunchId) RETURNING location_id";

        var @params = new
        {
            name = location.Name,
            bunchId = int.Parse(location.BunchId)
        };

        return (await _db.Insert(sql, @params)).ToString();
    }
    
    private Location CreateLocation(RawLocation rawLocation)
    {
        return new Location(
            rawLocation.Location_Id,
            rawLocation.Name,
            rawLocation.Bunch_Id);
    }
}