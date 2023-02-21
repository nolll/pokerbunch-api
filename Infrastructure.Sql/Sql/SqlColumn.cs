namespace Infrastructure.Sql.Sql;

public class SqlColumn
{
    private readonly string _table;
    private readonly string _columnName;
    public string AsParam() => _columnName;

    public SqlColumn(SqlTable table, string columnName)
    {
        _table = table;
        _columnName = columnName;
    }

    public override string ToString()
    {
        return $"{_table}.{_columnName}";
    }

    public static implicit operator string(SqlColumn column)
    {
        return column.ToString();
    }
}