using System.Security.Claims;
using Api.Auth;
using Api.Extensions;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class SettingsHandler
{
    public static async Task<IResult> Handle(RequireAppsettingsAccess requireAppsettingsAccess, ClaimsPrincipal user, AppSettings appSettings)
    {
        var result = await requireAppsettingsAccess.Execute(new RequireAppsettingsAccess.Request(new AuthWrapper(user).Principal));
        return ResultHandler.Model(result, () => appSettings);
    }
}