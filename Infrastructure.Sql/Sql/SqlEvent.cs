namespace Infrastructure.Sql.Sql;

public class SqlEvent : SqlTable
{
    public SqlColumn Id { get; }
    public SqlColumn BunchId { get; }
    public SqlColumn Name { get; }

    public SqlEvent() : base("pb_event")
    {
        Id = new SqlColumn(this, "event_id");
        BunchId = new SqlColumn(this, "bunch_id");
        Name = new SqlColumn(this, "name");
    }
}