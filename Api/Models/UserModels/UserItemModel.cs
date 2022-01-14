using System.Runtime.Serialization;
using Api.Extensions;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.UserModels;

[DataContract(Namespace = "", Name = "user")]
public class UserItemModel
{
    [DataMember(Name = "userName")]
    public string UserName { get; }

    [DataMember(Name = "displayName")]
    public string DisplayName { get; }

    [DataMember(Name = "url")]
    public string Url { get; }

    public UserItemModel(UserList.UserListItem r, UrlProvider urls)
        : this(r.UserName, r.DisplayName, urls.Api.User(r.UserName).Absolute())
    {
    }

    private UserItemModel(string userName, string displayName, string url)
    {
        UserName = userName;
        DisplayName = displayName;
        Url = url;
    }
}