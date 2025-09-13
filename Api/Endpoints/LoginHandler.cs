using Api.Auth;
using Api.Extensions;
using Api.Models.UserModels;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class LoginHandler
{
    public static async Task<IResult> Handle(Login login, AppSettings appSettings, [FromBody] LoginPostModel post)
    {
        var result = await login.Execute(new Login.Request(post.UserName, post.Password));

        if (result is { Success: true, Data: not null })
        {
            var token = TokenBuilder.CreateToken(result.Data, appSettings);
            var refreshToken = TokenBuilder.CreateRefreshToken(result.Data, appSettings);
            return ResultHandler.Success(new LoginModel(token, refreshToken));
        }

        return ResultHandler.Error(result.Error);
    }
}