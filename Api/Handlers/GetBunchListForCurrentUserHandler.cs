using System.Collections.Generic;
using System.Linq;
using Api.Auth;
using Api.Extensions;
using Api.Models.BunchModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetBunchListForCurrentUserHandler
{
    public static async Task<IResult> Handle(
        GetBunchListForUser getBunchListForUser,
        IAuth auth)
    {
        var result = await getBunchListForUser.Execute(new GetBunchListForUser.Request(auth.UserName));
        return ResultHandler.Model(result, CreateModel);
        IEnumerable<BunchModel>? CreateModel() => result.Data?.Bunches.Select(o => new BunchModel(o));
    }
}