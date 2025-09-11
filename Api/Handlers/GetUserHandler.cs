using System.Security.Claims;
using Api.Auth;
using Api.Extensions;
using Api.Models.UserModels;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetUserHandler
{
    public static async Task<IResult> Handle(UserDetails userDetails, ClaimsPrincipal user, string userName)
    {
        var currentUserName = new AuthWrapper(user).Principal.UserName;
        var result = await userDetails.Execute(new UserDetails.Request(currentUserName, userName));

        if (result.Data is null)
            return ResultHandler.Success(null);

        var canViewAll = result.Data?.CanViewAll ?? false;

        return ResultHandler.Model(result, () => canViewAll 
            ? new FullUserModel(result.Data!) 
            : new UserModel(result.Data!)
        );
    }
}