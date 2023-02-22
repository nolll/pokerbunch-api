using System.Linq;
using Infrastructure.Sql.Sql;
using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql;

public abstract class Db : IDb
{
    public abstract void Dispose();
    protected abstract QueryFactory QueryFactory { get; }

    public Task<IEnumerable<T>> GetAsync<T>(Query query)
    {
        return QueryFactory.FromQuery(query).GetAsync<T>();
    }

    public Task<T> FirstAsync<T>(Query query)
    {
        return QueryFactory.FromQuery(query).FirstAsync<T>();
    }

    public Task<T?> FirstOrDefaultAsync<T>(Query query)
    {
        return QueryFactory.FromQuery(query).FirstOrDefaultAsync<T?>();
    }

    public Task<int> InsertGetIdAsync(Query query, IDictionary<SqlColumn, object?> parameters)
    {
        return QueryFactory.FromQuery(query).InsertGetIdAsync<int>(ConvertParams(parameters));
    }

    public Task InsertAsync(Query query, IDictionary<SqlColumn, object?> parameters)
    {
        return QueryFactory.FromQuery(query).InsertAsync(ConvertParams(parameters));
    }

    public Task<int> UpdateAsync(Query query, IDictionary<SqlColumn, object?> parameters)
    {
        return QueryFactory.FromQuery(query).UpdateAsync(ConvertParams(parameters));
    }

    public Task<int> DeleteAsync(Query query)
    {
        return QueryFactory.FromQuery(query).DeleteAsync();
    }

    public Task<int> ExecuteSql(string sql)
    {
        return QueryFactory.StatementAsync(sql);
    }

    private static IDictionary<string, object?> ConvertParams(IDictionary<SqlColumn, object?> parameters)
    {
        return parameters.Keys.ToDictionary(key => key.AsParam(), key => parameters[key]);
    }
}