using System.Linq;
using Infrastructure.Sql.Interfaces;
using Npgsql;
using System.Data;

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

    public IStorageDataReader Query(string sql, IEnumerable<SimpleSqlParameter> parameters = null)
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

    public async Task<IStorageDataReader> QueryAsync(string sql, IEnumerable<SimpleSqlParameter> parameters = null)
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

    public IStorageDataReader Query(string sql, ListSqlParameter parameter)
    {
        var sqlWithIdList = sql.Replace(parameter.ParameterName, parameter.ParameterNameList);
        return Query(sqlWithIdList, parameter.ParameterList);
    }

    public async Task<IStorageDataReader> QueryAsync(string sql, ListSqlParameter parameter)
    {
        var sqlWithIdList = sql.Replace(parameter.ParameterName, parameter.ParameterNameList);
        return await QueryAsync(sqlWithIdList, parameter.ParameterList);
    }

    public int Execute(string sql, IEnumerable<SimpleSqlParameter> parameters = null)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        return command.ExecuteNonQuery();
    }

    public async Task<int> ExecuteAsync(string sql, IEnumerable<SimpleSqlParameter> parameters = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        await using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        return await command.ExecuteNonQueryAsync();
    }

    public int ExecuteInsert(string sql, IEnumerable<SimpleSqlParameter> parameters = null)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        return Convert.ToInt32(command.ExecuteScalar());
    }

    public async Task<int> ExecuteInsertAsync(string sql, IEnumerable<SimpleSqlParameter> parameters = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        await using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    private static NpgsqlParameter[] ToSqlCommands(IEnumerable<SimpleSqlParameter> parameters)
    {
        return parameters.Select(o => o.SqlParameter).ToArray();
    }
}