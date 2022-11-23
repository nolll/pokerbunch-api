using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class TimeStampSqlParameter : SimpleSqlParameter
{
    public TimeStampSqlParameter(string parameterName, DateTime value)
        : base(parameterName, DbType.DateTime, value)
    {
    }
}