namespace Infrastructure.Sql.Sql;

public class SqlCashgameCheckpoint : SqlTable
{
    public SqlColumn Id { get; }
    public SqlColumn CheckpointId { get; }
    public SqlColumn CashgameId { get; }
    public SqlColumn PlayerId { get; }
    public SqlColumn Type { get; }
    public SqlColumn Amount { get; }
    public SqlColumn Stack { get; }
    public SqlColumn Timestamp { get; }

    public SqlCashgameCheckpoint() : base("pb_cashgame_checkpoint")
    {
        Id = new SqlColumn(this, "cashgame_checkpoint_id");
        CheckpointId = new SqlColumn(this, "checkpoint_id");
        CashgameId = new SqlColumn(this, "cashgame_id");
        PlayerId = new SqlColumn(this, "player_id");
        Type = new SqlColumn(this, "type");
        Amount = new SqlColumn(this, "amount");
        Stack = new SqlColumn(this, "stack");
        Timestamp = new SqlColumn(this, "timestamp");
    }
}