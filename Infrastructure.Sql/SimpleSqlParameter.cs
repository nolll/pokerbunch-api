using Npgsql;
using System;
using System.Data;
using System.Globalization;
using NpgsqlTypes;

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

    public SimpleSqlParameter(string parameterName, bool value)
        : this(CreateSqlParameter(parameterName, DbType.Boolean, value))
    {
    }

    public SimpleSqlParameter(string parameterName, DateTime value)
        : this(CreateSqlParameter(parameterName, DbType.DateTime, value.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)))
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