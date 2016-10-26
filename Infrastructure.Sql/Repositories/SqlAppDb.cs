using System.Collections.Generic;
using Core.Entities;
using Infrastructure.Sql.Interfaces;

namespace Infrastructure.Sql.Repositories
{
    public class SqlAppDb
    {
        private const string DataSql = "SELECT a.ID, a.AppKey, a.Name, a.UserId FROM [App] a ";
        private const string SearchSql = "SELECT a.ID FROM [App] a ";

        private readonly SqlServerStorageProvider _db;

        public SqlAppDb(SqlServerStorageProvider db)
        {
            _db = db;
        }

        public IList<App> ListApps()
        {
            var ids = GetAppIdList();
            return GetAppList(ids);
        }

        public IList<App> ListApps(int userId)
        {
            var ids = GetAppIdList(userId);
            return GetAppList(ids);
        }

        public App Get(int id)
        {
            var sql = string.Concat(DataSql, "WHERE a.Id = @appId");
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@appId", id)
            };
            var reader = _db.Query(sql, parameters);
            return reader.ReadOne(CreateApp);
        }

        public IList<App> GetList(IList<int> ids)
        {
            return GetAppList(ids);
        }

        public IList<int> Find()
        {
            return GetAppIdList();
        }

        public IList<int> Find(int userId)
        {
            return GetAppIdList(userId);
        }

        public IList<int> Find(string appKey)
        {
            return GetAppIdList(appKey);
        }

        private IList<int> GetAppIdList()
        {
            var sql = string.Concat(SearchSql, "ORDER BY a.Name");
            var reader = _db.Query(sql);
            return reader.ReadIntList("ID");
        }

        public int Add(App app)
        {
            const string sql = "INSERT INTO [app] (AppKey, Name, UserId) VALUES (@appKey, @name, @userId) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
            var parameters = new List<SimpleSqlParameter>
		        {
		            new SimpleSqlParameter("@appKey", app.AppKey),
		            new SimpleSqlParameter("@name", app.Name),
		            new SimpleSqlParameter("@userId", app.UserId)
		        };
            return _db.ExecuteInsert(sql, parameters);
        }

        public void Update(App app)
        {
            throw new System.NotImplementedException();
        }

        private IList<int> GetAppIdList(int userId)
        {
            var sql = string.Concat(SearchSql, "WHERE a.UserId = @userId ORDER BY a.Name");
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@userId", userId)
            };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("ID");
        }

        private IList<int> GetAppIdList(string appKey)
        {
            var sql = string.Concat(DataSql, "WHERE a.AppKey = @appKey");
            var parameters = new List<SimpleSqlParameter>
            {
                new SimpleSqlParameter("@appKey", appKey)
            };
            var reader = _db.Query(sql, parameters);
            return reader.ReadIntList("ID");
        }

        private IList<App> GetAppList(IList<int> ids)
        {
            var sql = string.Concat(DataSql, "WHERE a.ID IN(@ids)");
            var parameter = new ListSqlParameter("@ids", ids);
            var reader = _db.Query(sql, parameter);
            return reader.ReadList(CreateApp);
        }

        private static App CreateApp(IStorageDataReader reader)
        {
            return new App(
                reader.GetIntValue("ID"),
                reader.GetStringValue("AppKey"),
                reader.GetStringValue("Name"),
                reader.GetIntValue("USerID"));
        }
    }
}