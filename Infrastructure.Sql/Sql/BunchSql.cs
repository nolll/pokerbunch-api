namespace Infrastructure.Sql.Sql;

public static class BunchSql
{
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
}