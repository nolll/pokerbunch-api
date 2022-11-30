using Infrastructure.Sql;
using Infrastructure.Sql.Interfaces;
using Infrastructure.Sql.SqlParameters;
using Microsoft.Data.Sqlite;
using Npgsql;
using System.Data;

namespace Tests.Integration;

public class SqliteDb : IDb
{
    private readonly SqliteConnection _connection;

    public SqliteDb(SqliteConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<IStorageDataReader> Query(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var command = new SqliteCommand(sql, _connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        var mySqlReader = await command.ExecuteReaderAsync();
        var dt = new DataTable();
        dt.Load(mySqlReader);
        return new StorageDataReader(dt.CreateDataReader());
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
            command.Parameters.AddRange(ToSqlCommands(parameters));

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> Insert(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var command = new SqliteCommand(sql, _connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    private static NpgsqlParameter[] ToSqlCommands(IEnumerable<SqlParam> parameters)
    {
        return parameters.Select(o => o.Parameter).ToArray();
    }
}