using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Infrastructure.Sql;

public class PostgresDb : Db
{
    private readonly string _connectionString;
    private readonly Compiler _compiler;
    protected override QueryFactory QueryFactory => new(GetConnection(), _compiler);

    public PostgresDb(string connectionString)
    {
        _connectionString = connectionString;
        _compiler = new PostgresCompiler();
    }
    
    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public override void Dispose()
    {
    }
}