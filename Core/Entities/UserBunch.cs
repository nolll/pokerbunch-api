namespace Core.Entities;

public class UserBunch(
    string slug,
    string name = "",
    string playerId = "",
    string playerName = "",
    Role role = Role.None)
{
    public string Slug { get; } = slug;
    public string Name { get; } = name;
    public string PlayerId { get; } = playerId;
    public string PlayerName { get; } = playerName;
    public Role Role { get; } = role;
}