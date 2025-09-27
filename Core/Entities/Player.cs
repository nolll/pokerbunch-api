namespace Core.Entities;

public class Player(
    string bunchSlug,
    string id,
    string? userId,
    string? userName,
    string displayName,
    Role role = Role.Player,
    string? color = null)
    : IEntity
{
    private const string DefaultColor = "#9e9e9e";
    
    public string BunchSlug { get; } = bunchSlug;
    public string Id { get; } = id;
    public string? UserId { get; } = userId;
    public string? UserName { get; } = userName;
    public string DisplayName { get; } = displayName;
    public Role Role { get; } = role;
    public string? Color { get; } = color ?? DefaultColor;
    public bool IsUser => UserId != default;

    public static Player New(string slug, string displayName, Role role = Role.Player, string? color = null) => 
        new(slug, "", null, null, displayName, role, color);

    public static Player New(string slug, string userId, string userName, Role role = Role.Player, string? color = null) => 
        new(slug, "", userId, userName, "", role, color);
}