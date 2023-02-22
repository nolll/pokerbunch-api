using Infrastructure.Sql;
using Microsoft.Data.Sqlite;
using Dapper;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Tests.Integration;

public class SqliteDb : IDb
{
    private readonly SqliteConnection _connection;
    public QueryFactory QueryFactory { get; }
    public Compiler Compiler { get; }
    public DbEngine Engine => DbEngine.Sqlite;

    public SqliteDb(string connectionString)
    {
        Compiler = new SqliteCompiler();
        _connection = new SqliteConnection(connectionString);
        _connection.Open(); // todo: don't reuse the same
        QueryFactory = new QueryFactory(_connection, Compiler);
    }
    
    public async Task<int> Execute(string sql, object? @params = null)
    {
        return await _connection.ExecuteAsync(sql, @params);
    }
    
    public void Dispose()
    {
        _connection.Dispose();
    }
}