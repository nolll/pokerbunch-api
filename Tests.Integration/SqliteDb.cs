using Infrastructure.Sql;
using Microsoft.Data.Sqlite;
using Dapper;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Tests.Integration;

public class SqliteDb : IDb
{
    private readonly SqliteConnection _connection;
    public QueryFactory QueryFactory { get; }
    public Compiler Compiler { get; }
    public DbEngine Engine => DbEngine.Sqlite;

    public SqliteDb(string connectionString)
    {
        Compiler = new SqliteCompiler();
        _connection = new SqliteConnection(connectionString);
        _connection.Open(); // todo: don't reuse the same
        QueryFactory = new QueryFactory(_connection, Compiler);
    }
    
    public async Task<T?> Single<T>(string sql, object @params)
    {
        return (await List<T>(sql, @params)).FirstOrDefault();
    }

    public async Task<T?> Single<T>(Query query)
    {
        return (await List<T>(query)).FirstOrDefault();
    }

    public async Task<IEnumerable<T>> List<T>(string sql, object? @params)
    {
        return await _connection.QueryAsync<T>(sql, @params);
    }

    public async Task<IEnumerable<T>> List<T>(Query query)
    {
        var compiledQuery = Compile(query);
        var result = await _connection.QueryAsync<T>(compiledQuery.Sql, compiledQuery.NamedBindings);
        return result;
    }

    public async Task<IEnumerable<T>> List<T>(string sql, ListParam param)
    {
        if (param.IdCount == 0)
            return Enumerable.Empty<T>();

        var sqlWithIdList = sql.Replace(param.Name, param.ParameterNameList);
        return await _connection.QueryAsync<T>(sqlWithIdList, param.DynamicParameters);
    }
    
    public async Task<int> Execute(string sql, object? @params = null)
    {
        return await _connection.ExecuteAsync(sql, @params);
    }

    public async Task<int> Insert(string sql, object? @params = null)
    {
        return await _connection.ExecuteScalarAsync<int>(sql, @params);
    }

    public async Task<int> Insert(Query query)
    {
        var compiledQuery = Compile(query);
        return await _connection.ExecuteScalarAsync<int>(compiledQuery.Sql, compiledQuery.NamedBindings);
    }

    private SqlResult Compile(Query query)
    {
        return Compiler.Compile(query);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}