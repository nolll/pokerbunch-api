using Infrastructure.Sql;
using Microsoft.Data.Sqlite;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Tests.Integration;

public class SqliteDb : Db
{
    private readonly SqliteConnection _connection;
    public override QueryFactory QueryFactory { get; }
    public Compiler Compiler { get; }

    public SqliteDb(string connectionString)
    {
        Compiler = new SqliteCompiler();
        _connection = new SqliteConnection(connectionString);
        _connection.Open();
        QueryFactory = new QueryFactory(_connection, Compiler);
    }
    
    public override void Dispose()
    {
        _connection.Dispose();
    }
}