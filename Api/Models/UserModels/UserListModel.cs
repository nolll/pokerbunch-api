using System.Collections.Generic;
using System.Linq;
using Api.Urls.ApiUrls;
using Core.UseCases;

namespace Api.Models.UserModels;

public class UserListModel : List<UserItemModel>
{
    public UserListModel(UserList.Result userList, UrlProvider urls)
    {
        AddRange(userList.Users.Select(o => new UserItemModel(o, urls)));
    }
}