using Npgsql;
using System;
using System.Globalization;

namespace Infrastructure.Sql;

public class SimpleSqlParameter
{
    public NpgsqlParameter SqlParameter { get; }

    public SimpleSqlParameter(string parameterName, string value)
        : this(CreateSqlParameter(parameterName, value))
    {
    }

    public SimpleSqlParameter(string parameterName, int value)
        : this(CreateSqlParameter(parameterName, value))
    {
    }

    public SimpleSqlParameter(string parameterName, bool value)
        : this(CreateSqlParameter(parameterName, value ? 1 : 0))
    {
    }

    public SimpleSqlParameter(string parameterName, DateTime value)
        : this(CreateSqlParameter(parameterName, value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)))
    {
    }

    private SimpleSqlParameter(NpgsqlParameter parameter)
    {
        SqlParameter = parameter;
    }

    private static NpgsqlParameter CreateSqlParameter(string parameterName, object value)
    {
        return new NpgsqlParameter(parameterName, value ?? DBNull.Value);
    }
}