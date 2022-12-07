using Infrastructure.Sql;
using Microsoft.Data.Sqlite;
using Dapper;

namespace Tests.Integration;

public class SqliteDb : IDb
{
    private readonly SqliteConnection _connection;
    public DbEngine Engine => DbEngine.Sqlite;

    public SqliteDb(string connectionString)
    {
        _connection = new SqliteConnection(connectionString);
        _connection.Open(); // todo: don't reuse the same connection
    }
    
    public async Task<T> Single<T>(string sql, object @params)
    {
        return (await List<T>(sql, @params)).FirstOrDefault();
    }

    public async Task<IEnumerable<T>> List<T>(string sql, object @params)
    {
        return await _connection.QueryAsync<T>(sql, @params);
    }

    public async Task<IEnumerable<T>> List<T>(string sql, ListParam param)
    {
        var sqlWithIdList = sql.Replace(param.Name, param.ParameterNameList);
        return await _connection.QueryAsync<T>(sqlWithIdList, param.DynamicParameters);
    }
    
    public async Task<int> Execute(string sql, object @params = null)
    {
        return await _connection.ExecuteAsync(sql, @params);
    }
    
    public async Task<int> Insert(string sql, object @params = null)
    {
        return await _connection.ExecuteScalarAsync<int>(sql, @params);
    }
    
    public void Dispose()
    {
        _connection?.Dispose();
    }
}