using System.Linq;
using Infrastructure.Sql.Interfaces;
using Npgsql;
using System.Data;
using Dapper;
using Infrastructure.Sql.SqlParameters;

namespace Infrastructure.Sql;

public class PostgresDb : IDb
{
    private readonly string _connectionString;
    public DbEngine Engine => DbEngine.Postgres;
    private IStorageDataReader GetDataReader(IDataReader dataReader) => new PostgresDataReader(dataReader);

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
            command.Parameters.AddRange(ToPostgresParams(parameters));

        var mySqlReader = await command.ExecuteReaderAsync();
        var dt = new DataTable();
        dt.Load(mySqlReader);
        return GetDataReader(dt.CreateDataReader());
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

    public async Task<IStorageDataReader> Query(string sql, ListParam parameter)
    {
        var sqlWithIdList = sql.Replace(parameter.Name, parameter.ParameterNameList);
        return await Query(sqlWithIdList, parameter.ParameterList);
    }

    public async Task<int> Execute(string sql, object @params = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        return await connection.ExecuteAsync(sql, @params);
    }

    public async Task<int> Insert(string sql, IEnumerable<SqlParam> parameters = null)
    {
        await using var connection = GetConnection();
        connection.Open();

        await using var command = new NpgsqlCommand(sql, connection);
        if (parameters != null)
            command.Parameters.AddRange(ToPostgresParams(parameters));

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
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

    private static NpgsqlParameter[] ToPostgresParams(IEnumerable<SqlParam> parameters)
    {
        return parameters.Select(ToPostgresParams).ToArray();
    }

    private static NpgsqlParameter ToPostgresParams(SqlParam p)
    {
        return new NpgsqlParameter(p.Name, p.Type)
        {
            Value = p.Value
        };
    }

    public void Dispose()
    {
    }
}