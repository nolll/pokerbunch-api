using System.Data;
using Npgsql;

namespace Infrastructure.Sql.SqlParameters;

public class SimpleSqlParameter
{
    public NpgsqlParameter SqlParameter { get; }
    
    public SimpleSqlParameter(string parameterName, DbType type, object value)
    {
        SqlParameter = new NpgsqlParameter(parameterName, type)
        {
            Value = value
        };
    }
}