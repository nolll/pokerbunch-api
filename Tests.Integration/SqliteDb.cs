using Infrastructure.Sql;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;

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

    public async Task<IStorageDataReader> Query(string sql, ListParam parameter)
    {
        var sqlWithIdList = sql.Replace(parameter.Name, parameter.ParameterNameList);
        return await Query(sqlWithIdList, parameter.ParameterList);
    }

    public async Task<int> Execute(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var command = new SqliteCommand(sql, _connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqliteParams(parameters));

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> Insert(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var command = new SqliteCommand(sql, _connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqliteParams(parameters));

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
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