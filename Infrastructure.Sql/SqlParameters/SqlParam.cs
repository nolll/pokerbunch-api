using System.Data;
using Npgsql;

namespace Infrastructure.Sql.SqlParameters;

public class SqlParam
{
    public NpgsqlParameter Parameter { get; }
    
    public SqlParam(string name, DbType type, object value)
    {
        Parameter = new NpgsqlParameter(name, type)
        {
            Value = value
        };
    }
}