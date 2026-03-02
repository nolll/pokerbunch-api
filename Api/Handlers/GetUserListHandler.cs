using System.Linq;
using Api.Extensions;
using Api.Models.UserModels;
using Core.Services;
using Core.Services.Interfaces;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetUserListHandler
{
    public static async Task<IResult> Handle(UserList userList, IAuth auth, IApiUrlProvider urls)
    {
        var result = await userList.Execute(new UserList.Request(auth));
        return ResultHandler.Model(result, () => result.Data?.Users.Select(o => new UserItemModel(o, urls)));
    }
}