namespace Infrastructure.Sql.Sql;

public class SqlCashgame : SqlTable
{
    public SqlColumn Id { get; }
    public SqlColumn BunchId { get; }
    public SqlColumn LocationId { get; }
    public SqlColumn Date { get; }
    public SqlColumn Status { get; }
    public SqlColumn Timestamp { get; }

    public SqlCashgame() : base("pb_cashgame")
    {
        Id = new SqlColumn(this, "cashgame_id");
        BunchId = new SqlColumn(this, "bunch_id");
        LocationId = new SqlColumn(this, "location_id");
        Date = new SqlColumn(this, "date");
        Status = new SqlColumn(this, "status");
        Timestamp = new SqlColumn(this, "timestamp");
    }
}