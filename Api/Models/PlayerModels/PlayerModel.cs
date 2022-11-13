using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.PlayerModels;

public class PlayerModel
{
    [JsonPropertyName("id")]
    public string Id { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("userId")]
    public string UserId { get; }

    [JsonPropertyName("userName")]
    public string UserName { get; }

    [JsonPropertyName("avatarUrl")]
    public string AvatarUrl { get; }

    [JsonPropertyName("bunchId")]
    public string Slug { get; }

    [JsonPropertyName("color")]
    public string Color { get; }

    public PlayerModel(GetPlayer.Result r)
    {
        Id = r.PlayerId.ToString();
        Name = r.DisplayName;
        Slug = r.Slug;
        UserId = r.UserId.ToString();
        UserName = r.UserName;
        AvatarUrl = r.AvatarUrl;
        Color = r.Color;
    }

    [JsonConstructor]
    public PlayerModel(string id, string name, string userId, string userName, string avatarUrl, string slug, string color)
    {
        Id = id;
        Name = name;
        UserId = userId;
        UserName = userName;
        AvatarUrl = avatarUrl;
        Slug = slug;
        Color = color;
    }
}