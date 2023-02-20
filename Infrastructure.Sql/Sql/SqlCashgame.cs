namespace Infrastructure.Sql.Sql;

public class SqlCashgame : SqlTable
{
    public SqlColumn Id { get; }
    public SqlColumn LocationId { get; }
    public SqlColumn Date { get; }
    public SqlColumn Timestamp { get; }

    public SqlCashgame() : base("pb_cashgame")
    {
        Id = new SqlColumn(this, "cashgame_id");
        LocationId = new SqlColumn(this, "location_id");
        Date = new SqlColumn(this, "date");
        Timestamp = new SqlColumn(this, "timestamp");
    }
}