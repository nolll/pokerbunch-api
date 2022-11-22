namespace Core.Entities;

public class Player : IEntity
{
    public string BunchId { get; }
    public string Id { get; }
    public string UserId { get; }
    public string UserName { get; }
    public string DisplayName { get; }
    public Role Role { get; }
    public string Color { get; }
    public bool IsUser => UserId != default;
    public const string DefaultColor = "#9e9e9e";

    public Player(
        string bunchId,
        string id, 
        string userId, 
        string userName,
        string displayName = null, 
        Role role = Role.Player,
        string color = null)
    {
        BunchId = bunchId;
        Id = id;
        UserId = userId;
        UserName = userName;
        DisplayName = displayName;
        Role = role;
        Color = color ?? DefaultColor;
    }

    public static Player New(string bunchId, string displayName, Role role = Role.Player, string color = null)
    {
        return new Player(bunchId, null, null, null, displayName, role, color);
    }

    public static Player New(string bunchId, string userId, string userName, Role role = Role.Player, string color = null)
    {
        return new Player(bunchId, null, userId, userName, null, role, color);
    }

    public bool IsInRole(Role requiredRole)
    {
        return Role >= requiredRole;
    }
}