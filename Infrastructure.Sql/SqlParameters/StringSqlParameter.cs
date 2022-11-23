namespace Infrastructure.Sql.SqlParameters;

public class StringSqlParameter : SimpleSqlParameter
{
    public StringSqlParameter(string parameterName, string value)
        : base(parameterName, value)
    {
    }
}