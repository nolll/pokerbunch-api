using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class DateParam : SqlParam
{
    public DateParam(string name, DateTime value)
        : base(name, DbType.Date, value)
    {
    }
}