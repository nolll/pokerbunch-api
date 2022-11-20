using System.Text.Json.Serialization;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.UserModels;

public class UserItemModel
{
    [JsonPropertyName("userName")]
    public string UserName { get; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; }

    [JsonPropertyName("url")]
    public string Url { get; }

    public UserItemModel(UserList.UserListItem r, UrlProvider urls)
        : this(r.UserName, r.DisplayName, urls.Api.User(r.UserName))
    {
    }

    [JsonConstructor]
    public UserItemModel(string userName, string displayName, string url)
    {
        UserName = userName;
        DisplayName = displayName;
        Url = url;
    }
}