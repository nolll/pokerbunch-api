using System.Security.Claims;
using Api.Auth;
using Api.Extensions;
using Api.Models.CommonModels;
using Api.Routes;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class ClearCacheHandler
{
    public static async Task<IResult> Handle(ClearCache clearCache, ClaimsPrincipal user)
    {
        var result = await clearCache.Execute(new ClearCache.Request(new AuthWrapper(user).Principal));
        return ResultHandler.Model(result, () => new MessageModel(result.Data?.Message));
    }
}