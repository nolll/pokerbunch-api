namespace Infrastructure.Sql.Sql;

public static class EventSql
{
    public const string SearchByIdQuery = @"
        SELECT e.event_id
        FROM pb_event e
        WHERE e.bunch_id = @id";

    public const string SearchByCashgameQuery = @"
        SELECT ecg.event_id
        FROM pb_event_cashgame ecg
        WHERE ecg.cashgame_id = @id";

    public const string AddQuery = @"
        INSERT INTO pb_event (name, bunch_id)
        VALUES (@name, @bunchId) RETURNING event_id";

    public const string AddCashgameQuery = @"
        INSERT INTO pb_event_cashgame (event_id, cashgame_id)
        VALUES (@eventId, @cashgameId)";

    public const string RemoveCashgameQuery = @"
        DELETE FROM pb_event_cashgame
        WHERE event_id = @eventId
        AND cashgame_id = @cashgameId";
}