using System.Data;
using System.Linq;

namespace Infrastructure.Sql.SqlParameters;

public class IntListParam : ListParam
{
    public IntListParam(string name, IList<string> idList)
        : base(name, DbType.Int32, idList.Select(o => (object)int.Parse(o)).ToList())
    {
    }
}
