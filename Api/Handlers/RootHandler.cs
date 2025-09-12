using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class RootHandler
{
    public static IResult Handle(TestEmail testEmail, AppSettings appSettings) => 
        Results.Redirect("/swagger/index.html");
}