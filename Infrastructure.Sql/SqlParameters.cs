namespace Infrastructure.Sql;

public class SqlParameters : List<SimpleSqlParameter>
{
    public SqlParameters(params SimpleSqlParameter[] list)
    {
        foreach (var parameter in list)
        {
            Add(parameter);
        }
    }
}