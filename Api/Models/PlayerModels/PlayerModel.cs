using System.Text.Json.Serialization;
using Core.UseCases;

namespace Api.Models.PlayerModels;

public class PlayerModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [JsonPropertyName("avatarUrl")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("bunchId")]
    public string Slug { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    public PlayerModel()
    {
    }

    public PlayerModel(GetPlayer.Result r)
    {
        Id = r.PlayerId;
        Name = r.DisplayName;
        Slug = r.Slug;
        UserId = r.UserId.ToString();
        UserName = r.UserName;
        AvatarUrl = r.AvatarUrl;
        Color = r.Color;
    }
}