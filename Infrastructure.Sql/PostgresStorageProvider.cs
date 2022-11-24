using System.Linq;
using Infrastructure.Sql.Interfaces;
using Npgsql;
using System.Data;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql;

public class PostgresStorageProvider
{
    private readonly string _connectionString;

    public PostgresStorageProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public IStorageDataReader Query(string sql, IEnumerable<SqlParam> parameters = null)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));
        
        var mySqlReader = command.ExecuteReader();
        var dt = new DataTable();
        dt.Load(mySqlReader);
        return new StorageDataReader(dt.CreateDataReader());
    }

    public async Task<IStorageDataReader> QueryAsync(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        await using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        var mySqlReader = await command.ExecuteReaderAsync();
        var dt = new DataTable();
        dt.Load(mySqlReader);
        return new StorageDataReader(dt.CreateDataReader());
    }

    public IStorageDataReader Query(string sql, ListParam parameter)
    {
        var sqlWithIdList = sql.Replace(parameter.Name, parameter.ParameterNameList);
        return Query(sqlWithIdList, parameter.ParameterList);
    }

    public async Task<IStorageDataReader> QueryAsync(string sql, ListParam parameter)
    {
        var sqlWithIdList = sql.Replace(parameter.Name, parameter.ParameterNameList);
        return await QueryAsync(sqlWithIdList, parameter.ParameterList);
    }

    public int Execute(string sql, IEnumerable<SqlParam> parameters = null)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        return command.ExecuteNonQuery();
    }

    public async Task<int> ExecuteAsync(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        await using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        return await command.ExecuteNonQueryAsync();
    }

    public int ExecuteInsert(string sql, IEnumerable<SqlParam> parameters = null)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        return Convert.ToInt32(command.ExecuteScalar());
    }

    public async Task<int> ExecuteInsertAsync(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        await using var command = new NpgsqlCommand(sql, connection);
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