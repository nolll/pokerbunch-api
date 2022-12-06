using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;

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

        var parameters = new List<SqlParam>
        {
            new IntParam("@id", id)
        };

        var reader = await _db.Query(sql, parameters);
        var rawLocation =  reader.ReadOne(CreateRawLocation);
        return rawLocation != null
            ? CreateLocation(rawLocation)
            : null;
    }
        
    public async Task<IList<Location>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Location>();

        var sql = string.Concat(DataSql, "WHERE l.location_id IN (@ids)");
        var parameter = new IntListParam("@ids", ids);
        var reader = await _db.Query(sql, parameter);
        var rawLocations = reader.ReadList(CreateRawLocation);
        return rawLocations.Select(CreateLocation).ToList();
    }

    public async Task<IList<string>> Find(string bunchId)
    {
        var sql = string.Concat(SearchIdSql, "WHERE l.bunch_id = @bunchId");
        var parameters = new List<SqlParam>
        {
            new IntParam("@bunchId", bunchId)
        };
        var reader = await _db.Query(sql, parameters);
        return reader.ReadIntList("location_id").Select(o => o.ToString()).ToList();
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

    private static RawLocation CreateRawLocation(IStorageDataReader reader)
    {
        return new RawLocation
        {
            Location_Id = reader.GetIntValue("location_id").ToString(),
            Name = reader.GetStringValue("name"),
            Bunch_Id = reader.GetIntValue("bunch_id").ToString()
        };
    }

    private Location CreateLocation(RawLocation rawLocation)
    {
        return new Location(
            rawLocation.Location_Id,
            rawLocation.Name,
            rawLocation.Bunch_Id);
    }
}