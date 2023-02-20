namespace Infrastructure.Sql.Sql;

public class SqlPlayer : SqlTable
{
    public SqlColumn BunchId { get; }
    public SqlColumn Id { get; }
    public SqlColumn UserId { get; }
    public SqlColumn RoleId { get; }
    public SqlColumn PlayerName { get; }
    public SqlColumn Color { get; }
    public SqlColumn UserName { get; }

    public SqlPlayer() : base("pb_player")
    {
        BunchId = new SqlColumn(this, "bunch_id");
        Id = new SqlColumn(this, "player_id");
        UserId = new SqlColumn(this, "user_id");
        RoleId = new SqlColumn(this, "role_id");
        PlayerName = new SqlColumn(this, "player_name");
        Color = new SqlColumn(this, "color");
        UserName = new SqlColumn(this, "user_name");
    }
}