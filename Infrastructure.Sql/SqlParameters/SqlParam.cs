using System.Data;
using Npgsql;

namespace Infrastructure.Sql.SqlParameters;

public class SqlParam
{
    public string Name { get; }
    public DbType Type { get; }
    public object Value { get; }

    public SqlParam(string name, DbType type, object value)
    {
        Name = name;
        Type = type;
        Value = value;
    }
}