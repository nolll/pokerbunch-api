using Npgsql;
using System.Data;

namespace Infrastructure.Sql;

public class SimpleSqlParameter
{
    public NpgsqlParameter SqlParameter { get; }

    public SimpleSqlParameter(string parameterName, string value)
        : this(CreateSqlParameter(parameterName, DbType.String, value))
    {
    }

    public SimpleSqlParameter(string parameterName, int value)
        : this(CreateSqlParameter(parameterName, DbType.Int32, value))
    {
    }
    
    public SimpleSqlParameter(string parameterName, DbType type, object value)
        : this(CreateSqlParameter(parameterName, type, value))
    {
    }

    private SimpleSqlParameter(NpgsqlParameter parameter)
    {
        SqlParameter = parameter;
    }

    private static NpgsqlParameter CreateSqlParameter(string parameterName, DbType type, object value)
    {
        var p = new NpgsqlParameter(parameterName, type)
        {
            Value = value
        };
        return p;
    }
}