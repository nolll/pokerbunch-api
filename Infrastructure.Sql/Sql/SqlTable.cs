namespace Infrastructure.Sql.Sql;

public class SqlTable(string tableName)
{
    private string TableName { get; } = tableName;
    public override string ToString() => TableName;
    public static implicit operator string(SqlTable table) => table.ToString();
}