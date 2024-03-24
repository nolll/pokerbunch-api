using System.Text.Json.Serialization;

namespace Api.Models.CashgameModels;

[method: JsonConstructor]
public class CashgameDetailsEventModel(string? id, string? name)
{
    [JsonPropertyName("id")]
    public string Id { get; } = id ?? "";

    [JsonPropertyName("name")]
    public string Name { get; } = name ?? "";
}