using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class TimestampParam : SqlParam
{
    public TimestampParam(string name, DateTime value)
        : base(name, DbType.DateTime, value)
    {
    }
}