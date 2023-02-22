using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql;

public abstract class Db : IDb
{
    public abstract void Dispose();
    public abstract QueryFactory QueryFactory { get; }

    public Task<IEnumerable<T>> GetAsync<T>(Query query)
    {
        return QueryFactory.FromQuery(query).GetAsync<T>();
    }

    public Task<int> UpdateAsync(Query query, IDictionary<string, object> parameters)
    {
        return QueryFactory.FromQuery(query).UpdateAsync(parameters);
    }

    public Task<int> DeleteAsync(Query query)
    {
        return QueryFactory.FromQuery(query).DeleteAsync();
    }

    public Task<int> ExecuteSql(string sql)
    {
        return QueryFactory.StatementAsync(sql);
    }
}