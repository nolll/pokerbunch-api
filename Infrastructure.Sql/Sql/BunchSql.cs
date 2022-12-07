namespace Infrastructure.Sql.Sql;

public static class BunchSql
{
    private const string GetQuery = """
        SELECT b.bunch_id, b.name, b.display_name, b.description, b.currency, b.currency_layout, b.timezone, b.default_buyin, b.cashgames_enabled, b.tournaments_enabled, b.videos_enabled, b.house_rules
        FROM pb_bunch b
        """;

    public static string GetByIdQuery => $"{GetQuery} WHERE bunch_id = @id";
    public static string GetByIdsQuery => $"{GetQuery} WHERE b.bunch_id IN(@ids)";

    public const string SearchQuery = """
        SELECT b.bunch_id
        FROM pb_bunch b
        """;

    public static string SearchBySlugQuery => $"{SearchQuery} WHERE b.name = @slug";
    public static string SearchByUserQuery => $"{SearchQuery} INNER JOIN pb_player p on b.bunch_id = p.bunch_id WHERE p.user_id = @userId ORDER BY b.name";

    public const string AddQuery = """
        INSERT INTO pb_bunch (name, display_name, description, currency, currency_layout, timezone, default_buyin, cashgames_enabled, tournaments_enabled, videos_enabled, house_rules)
        VALUES (@slug, @displayName, @description, @currencySymbol, @currencyLayout, @timeZone, 0, @cashgamesEnabled, @tournamentsEnabled, @videosEnabled, @houseRules) RETURNING bunch_id
        """;

    public const string UpdateQuery = """
        UPDATE pb_bunch
        SET name = @slug,
            display_name = @displayName,
            description = @description, 
            house_rules = @houseRules,
            currency = @currencySymbol,
            currency_layout = @currencyLayout,
            timezone = @timeZone,
            default_buyin = @defaultBuyin,
            cashgames_enabled = @cashgamesEnabled,
            tournaments_enabled = @tournamentsEnabled,
            videos_enabled = @videosEnabled
        WHERE bunch_id = @id
        """;

    public const string DeleteSql = """
        DELETE
        FROM pb_bunch
        WHERE bunch_id = @id
        """;
}