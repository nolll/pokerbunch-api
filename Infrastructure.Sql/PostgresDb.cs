using System.Linq;
using Npgsql;
using Dapper;

namespace Infrastructure.Sql;

public class PostgresDb : IDb
{
    private readonly string _connectionString;
    public DbEngine Engine => DbEngine.Postgres;
    
    public PostgresDb(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<T> Single<T>(string sql, object @params)
    {
        await using var connection = GetConnection();
        connection.Open();
        return (await List<T>(sql, @params)).FirstOrDefault();
    }

    public async Task<IEnumerable<T>> List<T>(string sql, object @params)
    {
        await using var connection = GetConnection();
        connection.Open();
        return await connection.QueryAsync<T>(sql, @params);
    }

    public async Task<IEnumerable<T>> List<T>(string sql, ListParam param)
    {
        var sqlWithIdList = sql.Replace(param.Name, param.ParameterNameList);
        await using var connection = GetConnection();
        connection.Open();
        return await connection.QueryAsync<T>(sqlWithIdList, param.DynamicParameters);
    }
    
    public async Task<int> Execute(string sql, object @params = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        return await connection.ExecuteAsync(sql, @params);
    }
    
    public async Task<int> Insert(string sql, object @params = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        return await connection.ExecuteScalarAsync<int>(sql, @params);
    }

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public void Dispose()
    {
    }
}