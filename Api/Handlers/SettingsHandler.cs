using System.Security.Claims;
using Api.Auth;
using Api.Extensions;
using Api.Settings;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class SettingsHandler
{
    public static async Task<IResult> Handle(RequireAppsettingsAccess requireAppsettingsAccess, IAuth auth, AppSettings appSettings)
    {
        var result = await requireAppsettingsAccess.Execute(new RequireAppsettingsAccess.Request(auth));
        return ResultHandler.Model(result, () => appSettings);
    }
}