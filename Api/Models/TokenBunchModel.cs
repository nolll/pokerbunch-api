using System.Text.Json.Serialization;
using Core.Entities;

namespace Api.Models;

public class TokenBunchModel(string id, string slug, string name, string playerId, string playerName, Role role)
{
    [JsonPropertyName("id")]
    public string Id { get; } = id;
    [JsonPropertyName("slug")]
    public string Slug { get; } = slug;
    [JsonPropertyName("name")]
    public string Name { get; } = name;
    [JsonPropertyName("playerId")]
    public string PlayerId { get; } = playerId;
    [JsonPropertyName("playerName")]
    public string PlayerName { get; } = playerName;
    [JsonPropertyName("role")]
    public Role Role { get; } = role;
}