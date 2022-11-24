using System.Data;

namespace Infrastructure.Sql.SqlParameters;

public class IntParam : SqlParam
{
    public IntParam(string name, int value)
        : base(name, DbType.Int32, value)
    {
    }

    public IntParam(string name, string value)
        : base(name, DbType.Int32, int.Parse(value))
    {
    }
}