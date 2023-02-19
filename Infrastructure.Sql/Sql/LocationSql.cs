using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql.Sql;

public static class SqlNames
{
    public static class Location
    {
        public const string Table = "pb_location";
        
        public static class Columns
        {
            public const string Id = "location_id";
            public const string Name = "name";
            public const string BunchId = "bunch_id";
        }
    }
}

public static class LocationQueries
{
    private static Query Get => new Query(SqlNames.Location.Table)
        .Select(SqlNames.Location.Columns.Id, SqlNames.Location.Columns.Name, SqlNames.Location.Columns.BunchId);

    public static Query GetById(int id) => Get.Where(SqlNames.Location.Columns.Id, id);
    public static Query GetByIds(IEnumerable<int> ids) => Get.Where(SqlNames.Location.Columns.Id, ids);

    private static Query Find => new Query(SqlNames.Location.Table)
        .Select(SqlNames.Location.Columns.Id);

    public static Query FindByBunch(int bunchId) => Find.Where(SqlNames.Location.Columns.BunchId, bunchId);

    public static Query Add(string name, int bunchId)
    {
        var parameters = new Dictionary<string, object>
        {
            { SqlNames.Location.Columns.Name, name },
            { SqlNames.Location.Columns.BunchId, bunchId }
        };

        return new Query(SqlNames.Location.Table).AsInsert(parameters);
    }
}

public static class LocationSql
{
    private const string GetQuery = @"
        SELECT l.location_id, l.name, l.bunch_id
        FROM pb_location l";

    private const string FindQuery = @"
        SELECT l.location_id
        FROM pb_location l";

    public static string GetByIdQuery => $"{GetQuery} WHERE l.location_id = @id";
    public static string GetByIdsQuery => $"{GetQuery} WHERE l.location_id IN (@ids)";
    public static string FindByBunch => $"{FindQuery} WHERE l.bunch_id = @bunchId";

    public const string AddQuery = @"
            INSERT INTO pb_location (name, bunch_id)
            VALUES (@name, @bunchId) RETURNING location_id";
}