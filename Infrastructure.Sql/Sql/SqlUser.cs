namespace Infrastructure.Sql.Sql;

public class SqlUser : SqlTable
{
    public SqlColumn Id { get; }
    public SqlColumn UserName { get; }
    public SqlColumn DisplayName { get; }
    public SqlColumn RealName { get; }
    public SqlColumn Email { get; }
    public SqlColumn Password { get; }
    public SqlColumn Salt { get; }
    public SqlColumn RoleId { get; }

    public SqlUser() : base("pb_user")
    {
        Id = new SqlColumn(this, "user_id");
        UserName = new SqlColumn(this, "user_name");
        DisplayName = new SqlColumn(this, "display_name");
        RealName = new SqlColumn(this, "real_name");
        Email = new SqlColumn(this, "email");
        Password = new SqlColumn(this, "password");
        Salt = new SqlColumn(this, "salt");
        RoleId = new SqlColumn(this, "role_id");
    }
}