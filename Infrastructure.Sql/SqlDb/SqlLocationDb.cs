using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Interfaces;

namespace Infrastructure.Sql.SqlDb
{
    public class SqlLocationDb
    {
        private const string DataSql = "SELECT l.Id, l.Name, l.BunchId FROM Location l ";
        private const string SearchIdSql = "SELECT l.Id FROM Location l ";

        private readonly SqlServerStorageProvider _db;

        public SqlLocationDb(SqlServerStorageProvider db)
        {
            _db = db;
        }

        public Location Get(int id)
        {
            var sql = string.Concat(DataSql, "WHERE l.Id = @id");
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@id", id)
            };
            var reader = _db.Query(sql, parameters);
            return reader.ReadOne(CreateLocation);
        }
        
        public IList<Location> Get(IList<int> ids)
        {
            if (!ids.Any())
                return new List<Location>();
            var sql = string.Concat(DataSql, "WHERE l.Id IN (@ids)");
            var parameter = new ListSqlParameter("@ids", ids);
            var reader = _db.Query(sql, parameter);
            return reader.ReadList(CreateLocation);
        }

        public IList<int> Find(int bunchId)
        {
            var sql = string.Concat(SearchIdSql, "WHERE l.BunchId = @bunchId");
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@bunchId", bunchId)
            };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("Id");
        }
        
        public int Add(Location location)
        {
            const string sql = "INSERT INTO location (Name, BunchId) VALUES (@name, @bunchId) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@name", location.Name),
                new SimpleSqlParameter("@bunchId", location.BunchId)
            };
            return _db.ExecuteInsert(sql, parameters);
        }

        private Location CreateLocation(IStorageDataReader reader)
        {
            return new Location(
                reader.GetIntValue("Id"),
                reader.GetStringValue("Name"),
                reader.GetIntValue("BunchId"));
        }
    }
}