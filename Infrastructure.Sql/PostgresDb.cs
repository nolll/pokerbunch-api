using Npgsql;
using Dapper;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Infrastructure.Sql;

public class PostgresDb : Db
{
    private readonly string _connectionString;
    public Compiler Compiler { get; }
    public override QueryFactory QueryFactory => new(GetConnection(), Compiler);

    public PostgresDb(string connectionString)
    {
        _connectionString = connectionString;
        Compiler = new PostgresCompiler();
    }
    
    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public override void Dispose()
    {
    }
}