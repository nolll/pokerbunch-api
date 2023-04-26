using Infrastructure.Sql.Sql;
using SqlKata;

namespace Infrastructure.Sql;

public interface IDb : IDisposable
{
    Task<IEnumerable<T>> GetAsync<T>(Query query);
    Task<T> FirstAsync<T>(Query query);
    Task<T?> FirstOrDefaultAsync<T>(Query query);
    Task<int> InsertGetIdAsync(Query query, IDictionary<SqlColumn, object?> parameters);
    Task InsertAsync(Query query, IDictionary<SqlColumn, object?> parameters);
    Task<int> UpdateAsync(Query query, IDictionary<SqlColumn, object?> parameters);
    Task<int> DeleteAsync(Query query);
    Task<int> ExecuteSql(string sql);
    string GetSql(Query query);
}