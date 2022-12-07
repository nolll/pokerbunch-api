namespace Infrastructure.Sql.Sql;

public static class EventSql
{
    private const string GetQuery = """
        SELECT e.event_id, e.bunch_id, e.name, c.location_id, cc.timestamp
        FROM pb_event e
        LEFT JOIN pb_event_cashgame ec
	        ON ec.event_id = e.event_id
        LEFT JOIN pb_cashgame c
	        ON ec.cashgame_id = c.cashgame_id
        LEFT JOIN pb_cashgame_checkpoint cc
	        ON cc.checkpoint_id = (
		        SELECT checkpoint_id
		        FROM pb_cashgame_checkpoint cc
		        WHERE cashgame_id = c.cashgame_id
		        ORDER BY cc.timestamp DESC
		        LIMIT 1
	        ) 
        {0}
        ORDER BY e.event_id, c.date
""";

    public static string GetByIdQuery => string.Format(GetQuery, "WHERE e.event_id = @eventId");
    public static string GetByIdsQuery => string.Format(GetQuery, "WHERE e.event_id IN (@ids)");

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