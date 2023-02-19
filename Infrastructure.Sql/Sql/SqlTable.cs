namespace Infrastructure.Sql.Sql;

public class SqlTable
{
    private string TableName { get; }

    protected SqlTable(string tableName)
    {
        TableName = tableName;
    }

    public override string ToString()
    {
        return TableName;
    }

    public static implicit operator string(SqlTable table)
    {
        return table.ToString();
    }
}