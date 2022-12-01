using System.Data;
using Infrastructure.Sql;

namespace Tests.Integration;

public class SqliteDataReader : StorageDataReader
{
    private readonly IDataReader _reader;

    public SqliteDataReader(IDataReader reader)
        : base(reader)
    {
        _reader = reader;
    }

    public override int GetIntValue(string key)
    {
        var ordinal = _reader.GetOrdinal(key);
        if (_reader.IsDBNull(ordinal))
            return default;

        return Convert.ToInt32(_reader.GetInt64(ordinal));
    }
}