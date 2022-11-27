using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class StringParam : SqlParam
{
    public StringParam(string name, string value)
        : base(name, DbType.String, value)
    {
    }
}