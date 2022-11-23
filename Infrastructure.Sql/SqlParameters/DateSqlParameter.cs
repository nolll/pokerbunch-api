using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class DateSqlParameter : SimpleSqlParameter
{
    public DateSqlParameter(string parameterName, DateTime value)
        : base(parameterName, DbType.Date, value)
    {
    }
}