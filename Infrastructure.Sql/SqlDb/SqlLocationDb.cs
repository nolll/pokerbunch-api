using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Interfaces;

namespace Infrastructure.Sql.SqlDb;

public class SqlLocationDb
{
    private const string DataSql = @"
        SELECT l.location_id, l.name, l.bunch_id
        FROM pb_location l ";

    private const string SearchIdSql = @"
        SELECT l.location_id
        FROM pb_location l ";

    private readonly PostgresStorageProvider _db;

    public SqlLocationDb(PostgresStorageProvider db)
    {
        _db = db;
    }

    public Location Get(int id)
    {
        var sql = string.Concat(DataSql, "WHERE l.location_id = @id");
        var parameters = new List<SimpleSqlParameter>
        {
            new("@id", id)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadOne(CreateLocation);
    }
        
    public IList<Location> Get(IList<int> ids)
    {
        if (!ids.Any())
            return new List<Location>();

        var sql = string.Concat(DataSql, "WHERE l.location_id IN (@ids)");
        var parameter = new ListSqlParameter("@ids", ids);
        var reader = _db.Query(sql, parameter);
        return reader.ReadList(CreateLocation);
    }

    public IList<int> Find(int bunchId)
    {
        var sql = string.Concat(SearchIdSql, "WHERE l.bunch_id = @bunchId");
        var parameters = new List<SimpleSqlParameter>
        {
            new("@bunchId", bunchId)
        };
        var reader = _db.Query(sql, parameters);
        return reader.ReadIntList("location_id");
    }
        
    public int Add(Location location)
    {
        const string sql = @"
            INSERT INTO pb_location (name, bunch_id)
            VALUES (@name, @bunchId) RETURNING location_id";
        var parameters = new List<SimpleSqlParameter>
        {
            new("@name", location.Name),
            new("@bunchId", location.BunchId)
        };
        return _db.ExecuteInsert(sql, parameters);
    }

    private Location CreateLocation(IStorageDataReader reader)
    {
        return new Location(
            reader.GetIntValue("location_id"),
            reader.GetStringValue("name"),
            reader.GetIntValue("bunch_id"));
    }
}