namespace Infrastructure.Sql.Sql;

public static class UserSql
{
    private const string GetQuery = @"
        SELECT u.user_id, u.user_name, u.display_name, u.real_name, u.email, u.password, u.salt, u.role_id
        FROM pb_user u";

    public static string GetByIdQuery => $"{GetQuery} WHERE u.user_id = @userId";
    public static string GetByIdsQuery => $"{GetQuery} WHERE u.user_id IN (@ids)";

    private const string FindQuery = @"
        SELECT u.user_id
        FROM pb_user u";

    public static string FindByUsernameOrEmailQuery => $"{FindQuery} WHERE (u.user_name = @query OR u.email = @query)";
    public static string FindAllQuery => $"{FindQuery} ORDER BY u.display_name";

    public const string UpdateQuery = @"
        UPDATE pb_user 
        SET display_name = @displayName,
            real_name = @realName,
            email = @email,
            password = @password,
            salt = @salt
        WHERE user_id = @userId";

    public const string AddQuery = @"
        INSERT INTO pb_user (user_name, display_name, email, role_id, password, salt)
        VALUES (@userName, @displayName, @email, 1, @password, @salt) RETURNING user_id";

    public const string DeleteQuery = @"
        DELETE FROM pb_user
        WHERE user_iD = @userId";
}