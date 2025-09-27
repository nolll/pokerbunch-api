using System.Linq;
using Dapper;
using Infrastructure.Sql.Sql;
using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql;

public abstract class Db : IDb
{
    public abstract void Dispose();
    protected abstract QueryFactory QueryFactory { get; }

    public Task<IEnumerable<T>> GetAsync<T>(Query query) => QueryFactory.FromQuery(query).GetAsync<T>();

    public Task<T> FirstAsync<T>(Query query) => QueryFactory.FromQuery(query).FirstAsync<T>();

    public Task<T?> FirstOrDefaultAsync<T>(Query query) => QueryFactory.FromQuery(query).FirstOrDefaultAsync<T?>();

    public Task<int> InsertGetIdAsync(Query query, IDictionary<SqlColumn, object?> parameters) => 
        QueryFactory.FromQuery(query).InsertGetIdAsync<int>(ConvertParams(parameters));

    public Task InsertAsync(Query query, IDictionary<SqlColumn, object?> parameters) => 
        QueryFactory.FromQuery(query).InsertAsync(ConvertParams(parameters));

    public Task<int> UpdateAsync(Query query, IDictionary<SqlColumn, object?> parameters) => 
        QueryFactory.FromQuery(query).UpdateAsync(ConvertParams(parameters));

    public Task<int> DeleteAsync(Query query) => QueryFactory.FromQuery(query).DeleteAsync();

    public Task<int> ExecuteSql(string sql, Dictionary<string, object?>? parameters = null) => QueryFactory.StatementAsync(sql, parameters);

    public Task<int> CustomInsert(string sql, Dictionary<string, object?>? parameters = null) => 
        QueryFactory.Connection.QuerySingleAsync<int>(sql, parameters);

    public string GetSql(Query query) => QueryFactory.Compiler.Compile(query).ToString();

    private static IDictionary<string, object?> ConvertParams(IDictionary<SqlColumn, object?> parameters) => 
        parameters.Keys.ToDictionary(key => key.AsParam(), key => parameters[key]);
}