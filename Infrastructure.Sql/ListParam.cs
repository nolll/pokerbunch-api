using System.Linq;
using Dapper;

namespace Infrastructure.Sql;

public class ListParam
{
    private readonly IEnumerable<int> _idList;
    
    public string Name { get; }
    public int IdCount => _idList.Count();

    public ListParam(string name, IEnumerable<int> idList)
    {
        Name = name;
        _idList = idList;
    }

    public string ParameterNameList
    {
        get
        {
            var list = new List<string>();
            for (var i = 0; i < _idList.Count(); i++)
            {
                list.Add(GetIdParameterName(i));
            }
            return string.Join(",", list);
        }
    }
    
    public DynamicParameters DynamicParameters
    {
        get
        {
            var dictionary = new Dictionary<string, object>();

            var i = 0;
            foreach (var id in _idList)
            {
                dictionary.Add($"@param{i}", id);
                i++;
            }

            return new DynamicParameters(dictionary);
        }
    }

    private string GetIdParameterName(int index)
    {
        return $"@param{index}";
    }
}