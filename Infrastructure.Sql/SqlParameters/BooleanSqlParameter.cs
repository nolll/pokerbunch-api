using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class BooleanSqlParameter : SimpleSqlParameter
{
    public BooleanSqlParameter(string parameterName, bool value)
        : base(parameterName, DbType.Boolean, value)
    {
    }
}