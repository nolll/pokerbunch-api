using System.Text.Json.Serialization;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class RefreshPostModel(string token)
{
    public string Token { get; } = token;
}