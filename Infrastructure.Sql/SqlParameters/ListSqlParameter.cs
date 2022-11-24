using System.Data;
using System.Linq;

namespace Infrastructure.Sql.SqlParameters;

public class ListSqlParameter
{
    public string ParameterName { get; }
    private readonly DbType _type;
    private readonly IList<object> _idList;

    protected ListSqlParameter(string parameterName, DbType type, IList<object> idList)
    {
        ParameterName = parameterName;
        _type = type;
        _idList = idList;
    }

    public string ParameterNameList
    {
        get
        {
            var list = new List<string>();
            for (var i = 0; i < _idList.Count; i++)
            {
                list.Add(GetIdParameterName(i));
            }
            return string.Join(",", list);
        }
    }

    public IList<SimpleSqlParameter> ParameterList
    {
        get
        {
            return _idList.Select((t, i) => new SimpleSqlParameter(GetIdParameterName(i), _type, t)).ToList();
        }
    }

    private string GetIdParameterName(int index)
    {
        return $"@param{index}";
    }
}