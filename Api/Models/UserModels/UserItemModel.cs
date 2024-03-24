using System.Text.Json.Serialization;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.UserModels;

[method: JsonConstructor]
public class UserItemModel(string userName, string displayName, string url)
{
    [JsonPropertyName("userName")]
    public string UserName { get; } = userName;

    [JsonPropertyName("displayName")]
    public string DisplayName { get; } = displayName;

    [JsonPropertyName("url")]
    public string Url { get; } = url;

    public UserItemModel(UserList.UserListItem r, UrlProvider urls)
        : this(r.UserName, r.DisplayName, urls.Api.User(r.UserName))
    {
    }
}