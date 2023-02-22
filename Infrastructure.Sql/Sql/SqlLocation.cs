namespace Infrastructure.Sql.Sql;

public class SqlLocation : SqlTable
{
    public SqlColumn Id { get; }
    public SqlColumn Name { get; }
    public SqlColumn BunchId { get; }

    public SqlLocation() : base("pb_location")
    {
        Id = new SqlColumn(this, "location_id");
        Name = new SqlColumn(this, "name");
        BunchId = new SqlColumn(this, "bunch_id");
    }
}