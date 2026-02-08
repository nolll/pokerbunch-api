using Api.Extensions;
using Api.Models.UserModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetCurrentUserHandler
{
    public static async Task<IResult> Handle(UserDetails userDetails, IAuth auth)
    {
        var result = await userDetails.Execute(new UserDetails.Request(auth));
        return ResultHandler.Model(result, () => result.Data is not null ? new FullUserModel(result.Data) : null);
    }
}