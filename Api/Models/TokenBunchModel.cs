using System.Text.Json.Serialization;

namespace Api.Models;

public class TokenBunchModel(string id, string slug, string name, string playerId, string playerName, string role)
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
    public string Role { get; } = role;
}