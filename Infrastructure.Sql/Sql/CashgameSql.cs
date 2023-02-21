namespace Infrastructure.Sql.Sql;

public static class CashgameSql
{
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