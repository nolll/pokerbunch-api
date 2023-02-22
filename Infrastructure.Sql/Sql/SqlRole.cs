namespace Infrastructure.Sql.Sql;

public class SqlRole : SqlTable
{
    public SqlColumn Id { get; }
    public SqlColumn Name { get; }

    public SqlRole() : base("pb_role")
    {
        Id = new SqlColumn(this, "role_id");
        Name = new SqlColumn(this, "role_name");
    }
}