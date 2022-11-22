namespace Infrastructure.Sql.Classes;

public class RawPlayer
{
    public string BunchId { get; }
    public string Id { get; }
    public string UserId { get; }
    public string UserName { get; }
    public string DisplayName { get; }
    public int Role { get; }
    public string Color { get; }

    public RawPlayer(string bunchId, string id, string userId, string userName, string displayName, int role, string color)
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