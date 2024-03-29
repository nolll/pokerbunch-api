namespace Infrastructure.Sql.Sql;

public class SqlColumn : IEqualityComparer<SqlColumn>
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

    public bool Equals(SqlColumn? x, SqlColumn? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x._table == y._table && x._columnName == y._columnName;
    }

    public int GetHashCode(SqlColumn obj)
    {
        return HashCode.Combine(obj._table, obj._columnName);
    }
}