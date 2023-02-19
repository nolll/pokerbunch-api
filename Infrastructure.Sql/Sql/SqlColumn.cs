namespace Infrastructure.Sql.Sql;

public class SqlColumn
{
    private readonly SqlTable _table;
    public string ColumnName { get; }
    public string FullName => $"{_table}.{ColumnName}";

    public SqlColumn(SqlTable table, string columnName)
    {
        _table = table;
        ColumnName = columnName;
    }

    public override string ToString()
    {
        return ColumnName;
    }

    public static implicit operator string(SqlColumn column)
    {
        return column.ToString();
    }
}