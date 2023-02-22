using Infrastructure.Sql;
using Microsoft.Data.Sqlite;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Tests.Integration;

public class SqliteDb : Db
{
    private readonly SqliteConnection _connection;
    protected override QueryFactory QueryFactory { get; }

    public SqliteDb(string connectionString)
    {
        Compiler compiler = new SqliteCompiler();
        _connection = new SqliteConnection(connectionString);
        _connection.Open();
        QueryFactory = new QueryFactory(_connection, compiler);
    }
    
    public override void Dispose()
    {
        _connection.Dispose();
    }
}