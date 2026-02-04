namespace Infrastructure.Sql.Sql;

public class SqlJoinRequest : SqlTable
{
    public SqlColumn Id { get; }
    public SqlColumn BunchId { get; }
    public SqlColumn UserId { get; }

    public SqlJoinRequest() : base("pb_joinrequest")
    {
        Id = new SqlColumn(this, "request_id");
        BunchId = new SqlColumn(this, "bunch_id");
        UserId = new SqlColumn(this, "user_id");
    }
}