using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class StringSqlParameter : SimpleSqlParameter
{
    public StringSqlParameter(string parameterName, string value)
        : base(parameterName, DbType.String, value)
    {
    }
}