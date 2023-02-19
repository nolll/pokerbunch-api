namespace Infrastructure.Sql.Sql;

public class SqlPlayer : SqlTable
{
    public SqlColumn BunchId { get; }
    public SqlColumn UserId { get; }

    public SqlPlayer() : base("pb_player")
    {
        BunchId = new SqlColumn(this, "bunch_id");
        UserId = new SqlColumn(this, "user_id");
    }
}