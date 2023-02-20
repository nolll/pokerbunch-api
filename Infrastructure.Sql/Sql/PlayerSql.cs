namespace Infrastructure.Sql.Sql;

public static class PlayerSql
{
    private const string FindQuery = @"
        SELECT p.player_id
        FROM pb_player p ";

    public static string FindByBunchQuery => $"{FindQuery} WHERE p.bunch_id = @bunchId";
    public static string FindByUserQuery => $"{FindQuery} WHERE p.bunch_id = @bunchId AND p.user_id = @userId";
    
    public const string AddQuery = @"
        INSERT INTO pb_player (bunch_id, role_id, approved, player_name, color)
        VALUES (@bunchId, @role, @approved, @playerName, @color) RETURNING player_id";

    public const string AddWithUserQuery = @"
        INSERT INTO pb_player (bunch_id, user_id, role_id, approved, color)
        VALUES (@bunchId, @userId, @role, @approved, @color) RETURNING player_id";

    public const string UpdateQuery = @"
        UPDATE pb_player
        SET bunch_id = @bunchId,
            player_name = NULL,
            user_id = @userId,
            role_id = @role,
            approved = @approved
        WHERE player_id = @playerId";

    public const string DeleteQuery = @"
        DELETE FROM pb_player
        WHERE player_id = @playerId";
}