using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class BoolParam : SqlParam
{
    public BoolParam(string name, bool value)
        : base(name, DbType.Boolean, value)
    {
    }
}