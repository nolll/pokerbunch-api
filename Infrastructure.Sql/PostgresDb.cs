using System.Linq;
using Infrastructure.Sql.Interfaces;
using Npgsql;
using System.Data;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql;

public class PostgresDb : IDb
{
    private readonly string _connectionString;

    public DbEngine Engine => DbEngine.Postgres;

    public PostgresDb(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IStorageDataReader> Query(string sql, IEnumerable<SqlParam> parameters = null)
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

    public async Task<IStorageDataReader> Query(string sql, ListParam parameter)
    {
        var sqlWithIdList = sql.Replace(parameter.Name, parameter.ParameterNameList);
        return await Query(sqlWithIdList, parameter.ParameterList);
    }

    public async Task<int> Execute(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        await using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> Insert(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        await using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToSqlCommands(parameters));

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    private static NpgsqlParameter[] ToSqlCommands(IEnumerable<SqlParam> parameters)
    {
        return parameters.Select(o => o.Parameter).ToArray();
    }
}