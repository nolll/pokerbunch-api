using System.Linq;

namespace Infrastructure.Sql;

public class ListSqlParameter
{
    public string ParameterName { get; private set; }
    private readonly IList<int> _idList;

    public ListSqlParameter(string parameterName, IList<int> idList)
    {
        ParameterName = parameterName;
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
            return _idList.Select((t, i) => new SimpleSqlParameter(GetIdParameterName(i), t)).ToList();
        }
    }

    private string GetIdParameterName(int index)
    {
        return $"@param{index}";
    }
}