using Api.Extensions;
using Api.Models.HomeModels;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints;

public static class VersionHandler
{
    public static IResult Handle(TestEmail testEmail, AppSettings appSettings) => 
        ResultHandler.Success(new VersionModel(appSettings.Version));
}