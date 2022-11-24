using System.Linq;
using Core.Entities;
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

    private readonly PostgresDb _db;

    public SqlLocationDb(PostgresDb db)
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
        return reader.ReadOne(CreateLocation);
    }
        
    public async Task<IList<Location>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Location>();

        var sql = string.Concat(DataSql, "WHERE l.location_id IN (@ids)");
        var parameter = new IntListParam("@ids", ids);
        var reader = await _db.Query(sql, parameter);
        return reader.ReadList(CreateLocation);
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
        var parameters = new List<SqlParam>
        {
            new StringParam("@name", location.Name),
            new IntParam("@bunchId", location.BunchId)
        };
        return (await _db.Insert(sql, parameters)).ToString();
    }

    private Location CreateLocation(IStorageDataReader reader)
    {
        return new Location(
            reader.GetIntValue("location_id").ToString(),
            reader.GetStringValue("name"),
            reader.GetIntValue("bunch_id").ToString());
    }
}