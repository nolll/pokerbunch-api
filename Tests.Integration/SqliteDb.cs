using Infrastructure.Sql;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;
using Dapper;

namespace Tests.Integration;

public class SqliteDb : IDb
{
    private readonly SqliteConnection _connection;
    public DbEngine Engine => DbEngine.Sqlite;
    private IStorageDataReader GetDataReader(IDataReader dataReader) => new SqliteDataReader(dataReader);

    public SqliteDb(string connectionString)
    {
        _connection = new SqliteConnection(connectionString);
        _connection.Open();
    }

    public async Task<IStorageDataReader> Query(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var command = new SqliteCommand(sql, _connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqliteParams(parameters));

        var mySqlReader = await command.ExecuteReaderAsync();
        var dt = new DataTable();
        dt.Load(mySqlReader);
        return GetDataReader(dt.CreateDataReader());
    }

    public async Task<T> Single<T>(string sql, object @params)
    {
        return (await List<T>(sql, @params)).FirstOrDefault();
    }

    public async Task<IEnumerable<T>> List<T>(string sql, object @params)
    {
        return await _connection.QueryAsync<T>(sql, @params);
    }

    public async Task<IStorageDataReader> Query(string sql, ListParam parameter)
    {
        var sqlWithIdList = sql.Replace(parameter.Name, parameter.ParameterNameList);
        return await Query(sqlWithIdList, parameter.ParameterList);
    }
    
    public async Task<int> Execute(string sql, object @params = null)
    {
        return await _connection.ExecuteAsync(sql, @params);
    }
    
    public async Task<int> Insert(string sql, object @params = null)
    {
        return await _connection.ExecuteScalarAsync<int>(sql, @params);
    }

    private static SqliteParameter[] ToSqliteParams(IEnumerable<SqlParam> parameters)
    {
        return parameters.Select(ToSqliteParam).ToArray();
    }

    private static SqliteParameter ToSqliteParam(SqlParam p)
    {
        return new SqliteParameter(p.Name, p.Type)
        {
            Value = p.Value
        };
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}