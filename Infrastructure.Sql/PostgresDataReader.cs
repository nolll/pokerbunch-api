using System.Data;

namespace Infrastructure.Sql;

public class PostgresDataReader : StorageDataReader
{
    public PostgresDataReader(IDataReader reader)
        : base(reader)
    {
    }
}