using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql;

public interface IDb
{
    Task<IStorageDataReader> Query(string sql, IEnumerable<SqlParam> parameters = null);
    Task<IStorageDataReader> Query(string sql, ListParam parameter);
    Task<int> Execute(string sql, IEnumerable<SqlParam> parameters = null);
    Task<int> Insert(string sql, IEnumerable<SqlParam> parameters = null);
}