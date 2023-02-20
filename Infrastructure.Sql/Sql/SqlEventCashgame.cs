namespace Infrastructure.Sql.Sql;

public class SqlEventCashgame : SqlTable
{
    public SqlColumn EventId { get; }
    public SqlColumn CashgameId { get; }

    public SqlEventCashgame() : base("pb_event_cashgame")
    {
        EventId = new SqlColumn(this, "event_id");
        CashgameId = new SqlColumn(this, "cashgame_id");
    }
}