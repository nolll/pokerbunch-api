using Core.Entities;

namespace Api.Auth;

public class TokenBunch(string id, string name, string playerId, string playerName, Role role)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string PlayerId { get; } = playerId;
    public string PlayerName { get; } = playerName;
    public Role Role { get; } = role;
}