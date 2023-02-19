using System.Linq;
using Npgsql;
using Dapper;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Infrastructure.Sql;

public class PostgresDb : IDb
{
    private readonly string _connectionString;
    private readonly PostgresCompiler _compiler;
    public DbEngine Engine => DbEngine.Postgres;
    public QueryFactory QueryFactory => new(GetConnection(), _compiler);

    public PostgresDb(string connectionString)
    {
        _connectionString = connectionString;
        _compiler = new PostgresCompiler();
    }

    public async Task<T?> Single<T>(string sql, object @params)
    {
        await using var connection = GetConnection();
        connection.Open();
        return (await List<T>(sql, @params)).FirstOrDefault();
    }

    public async Task<T?> Single<T>(Query query)
    {
        await using var connection = GetConnection();
        connection.Open();
        return (await List<T>(query)).FirstOrDefault();
    }

    public async Task<IEnumerable<T>> List<T>(string sql, object? @params)
    {
        await using var connection = GetConnection();
        connection.Open();
        return await connection.QueryAsync<T>(sql, @params);
    }

    public async Task<IEnumerable<T>> List<T>(Query query)
    {
        await using var connection = GetConnection();
        connection.Open();
        var compiledQuery = Compile(query);
        return await connection.QueryAsync<T>(compiledQuery.Sql, compiledQuery.Bindings);
    }

    public async Task<IEnumerable<T>> List<T>(string sql, ListParam param)
    {
        if (param.IdCount == 0)
            return Enumerable.Empty<T>();

        var sqlWithIdList = sql.Replace(param.Name, param.ParameterNameList);
        await using var connection = GetConnection();
        connection.Open();
        return await connection.QueryAsync<T>(sqlWithIdList, param.DynamicParameters);
    }
    
    public async Task<int> Execute(string sql, object? @params = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        return await connection.ExecuteAsync(sql, @params);
    }

    public async Task<int> Insert(string sql, object? @params = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        return await connection.ExecuteScalarAsync<int>(sql, @params);
    }

    public async Task<int> Insert(Query query)
    {
        await using var connection = GetConnection();
        connection.Open();

        var compiledQuery = _compiler.Compile(query);
        return await connection.ExecuteScalarAsync<int>(compiledQuery.Sql, compiledQuery.NamedBindings);
    }

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    private SqlResult Compile(Query query)
    {
        return _compiler.Compile(query);
    }

    public void Dispose()
    {
    }
}