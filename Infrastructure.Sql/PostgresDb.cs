using Npgsql;
using Dapper;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Infrastructure.Sql;

public class PostgresDb : IDb
{
    private readonly string _connectionString;
    public Compiler Compiler { get; }
    public DbEngine Engine => DbEngine.Postgres;
    public QueryFactory QueryFactory => new(GetConnection(), Compiler);

    public PostgresDb(string connectionString)
    {
        _connectionString = connectionString;
        Compiler = new PostgresCompiler();
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

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public void Dispose()
    {
    }
}