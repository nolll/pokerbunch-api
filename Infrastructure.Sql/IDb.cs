using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql;

public interface IDb : IDisposable
{
    DbEngine Engine { get; }
    Task<IStorageDataReader> Query(string sql, IEnumerable<SqlParam> parameters = null);
    Task<T> Single<T>(string sql, object @params);
    Task<IEnumerable<T>> List<T>(string sql, object @params = null);
    Task<IStorageDataReader> Query(string sql, ListParam parameter);
    Task<int> Execute(string sql, object @params = null);
    Task<int> Insert(string sql, object @params = null);
}