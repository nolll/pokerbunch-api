using System.Data;
using System.Linq;

namespace Infrastructure.Sql.SqlParameters;

public class ListParam
{
    public string Name { get; }
    private readonly DbType _type;
    private readonly IList<object> _idList;

    protected ListParam(string name, DbType type, IList<object> idList)
    {
        Name = name;
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

    public IList<SqlParam> ParameterList
    {
        get
        {
            return _idList.Select((t, i) => new SqlParam(GetIdParameterName(i), _type, t)).ToList();
        }
    }

    private string GetIdParameterName(int index)
    {
        return $"@param{index}";
    }
}