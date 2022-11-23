using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class IntSqlParameter : SimpleSqlParameter
{
    public IntSqlParameter(string parameterName, int value)
        : base(parameterName, DbType.Int32, value)
    {
    }

    public IntSqlParameter(string parameterName, string value)
        : base(parameterName, DbType.Int32, int.Parse(value))
    {
    }
}