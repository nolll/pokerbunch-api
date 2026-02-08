using Api.Extensions;
using Api.Models.UserModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetUserHandler
{
    public static async Task<IResult> Handle(UserDetails userDetails, IAuth auth, string userName)
    {
        var result = await userDetails.Execute(new UserDetails.Request(auth, userName));

        if (result.Data is null)
            return ResultHandler.Success(null);

        var canViewAll = result.Data?.CanViewAll ?? false;

        return ResultHandler.Model(result, () => canViewAll 
            ? new FullUserModel(result.Data!) 
            : new UserModel(result.Data!)
        );
    }
}