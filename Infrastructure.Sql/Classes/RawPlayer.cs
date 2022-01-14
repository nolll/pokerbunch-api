namespace Infrastructure.Sql.Classes;

public class RawPlayer
{
    public int BunchId { get; }
    public int Id { get; }
    public int UserId { get; }
    public string UserName { get; }
    public string DisplayName { get; }
    public int Role { get; }
    public string Color { get; }

    public RawPlayer(int bunchId, int id, int userId, string userName, string displayName, int role, string color)
    {
        BunchId = bunchId;
        Id = id;
        UserId = userId;
        UserName = userName;
        DisplayName = displayName;
        Role = role;
        Color = color;
    }
}