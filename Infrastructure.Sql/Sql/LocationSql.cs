namespace Infrastructure.Sql.Sql;

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