using System.Data;
using System.Linq;

namespace Infrastructure.Sql.SqlParameters;

public class IntListSqlParameter : ListSqlParameter
{
    public IntListSqlParameter(string parameterName, IList<string> idList)
        : base(parameterName, DbType.Int32, idList.Select(o => (object)int.Parse(o)).ToList())
    {
    }
}
