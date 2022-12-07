namespace Infrastructure.Sql.Sql;

public static class CashgameSql
{
    private const string GetQuery = @"
        SELECT g.cashgame_id, g.bunch_id, g.location_id, ecg.event_id, g.status
        FROM pb_cashgame g
        LEFT JOIN pb_event_cashgame ecg ON ecg.cashgame_id = g.cashgame_id";

    public static string GetByIdQuery => $"{GetQuery} WHERE g.cashgame_id = @cashgameId ORDER BY g.cashgame_id";
    public static string GetByIdsQuery => $"{GetQuery} WHERE g.cashgame_id IN (@ids) ORDER BY g.cashgame_id";

    private const string SearchQuery = @"
        SELECT g.cashgame_id
        FROM pb_cashgame g";

    public static string SearchByBunchAndStatusQuery => $"{SearchQuery} WHERE g.bunch_id = @bunchId AND g.status = @status";
    public static string SearchByBunchAndStatusAndYearQuery(DbEngine engine) => engine == DbEngine.Postgres
        ? $"{SearchByBunchAndStatusQuery} AND DATE_PART('year', g.date) = @year"
        : $"{SearchByBunchAndStatusQuery} AND strftime('%Y', g.date) = cast(@year as text)";

    public static string SearchByEvent => $"{SearchQuery} WHERE g.cashgame_id IN (SELECT ecg.cashgame_id FROM pb_event_cashgame ecg WHERE ecg.event_id = @eventId)";

    public const string SearchByCheckpointSql = @"
        SELECT cp.cashgame_id
        FROM pb_cashgame_checkpoint cp
        WHERE cp.checkpoint_id = @checkpointId";

    public const string SearchByPlayerIdQuery = @"
        SELECT DISTINCT cashgame_id
        FROM pb_cashgame_checkpoint
        WHERE player_id = @playerId";

    public const string DeleteQuery = "DELETE FROM pb_cashgame WHERE cashgame_id = @cashgameId";

    public const string AddQuery = @"
        INSERT INTO pb_cashgame (bunch_id, location_id, status, date)
        VALUES (@bunchId, @locationId, @status, @date) RETURNING cashgame_id";

    public const string UpdateQuery = @"
        UPDATE pb_cashgame
        SET location_id = @locationId, status = @status
        WHERE cashgame_id = @cashgameId";

    public const string AddCheckpointQuery = @"
        INSERT INTO pb_cashgame_checkpoint (cashgame_id, player_id, type, amount, stack, timestamp)
        VALUES (@cashgameId, @playerId, @type, @amount, @stack, @timestamp) RETURNING checkpoint_id";

    public const string UpdateCheckpointQuery = @"
        UPDATE pb_cashgame_checkpoint
        SET timestamp = @timestamp,
            amount = @amount,
            stack = @stack
        WHERE checkpoint_id = @checkpointId";

    public const string DeleteCheckpointQuery = @"
        DELETE FROM pb_cashgame_checkpoint
        WHERE checkpoint_id = @checkpointId";

    public const string GetCheckpointsByCashgameQuery = @"
        SELECT cp.cashgame_id, cp.checkpoint_id, cp.player_id, cp.type, cp.stack, cp.amount, cp.timestamp
        FROM pb_cashgame_checkpoint cp
        WHERE cp.cashgame_id = @cashgameId
        ORDER BY cp.player_id, cp.timestamp, cp.checkpoint_id";

    public const string GetCheckpointsByCashgamesQuery = @"
        SELECT cp.cashgame_id, cp.checkpoint_id, cp.player_id, cp.type, cp.stack, cp.amount, cp.timestamp
        FROM pb_cashgame_checkpoint cp
        WHERE cp.cashgame_id IN (@cashgameIds)
        ORDER BY cp.player_id, cp.timestamp, cp.checkpoint_id DESC";
}