namespace Infrastructure.Sql.Sql;

public static class CashgameSql
{
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