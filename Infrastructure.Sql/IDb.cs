using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql;

public interface IDb : IDisposable
{
    QueryFactory QueryFactory { get; }
    Task<IEnumerable<T>> GetAsync<T>(Query query);
    Task<int> UpdateAsync(Query query, IDictionary<string, object> parameters);
    Task<int> DeleteAsync(Query query);
    Task<int> ExecuteSql(string sql);
}