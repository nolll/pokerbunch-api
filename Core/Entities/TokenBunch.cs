namespace Core.Entities;

public class TokenBunch(string id, string slug, string name, string playerId, string playerName, Role role)
{
    public string Id { get; } = id;
    public string Slug { get; } = slug;
    public string Name { get; } = name;
    public string PlayerId { get; } = playerId;
    public string PlayerName { get; } = playerName;
    public Role Role { get; } = role;
}