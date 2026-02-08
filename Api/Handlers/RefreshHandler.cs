using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Api.Auth;
using Api.Extensions;
using Api.Models.UserModels;
using Api.Settings;
using Core.Errors;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Handlers;

public static class RefreshHandler
{
    public static async Task<IResult> Handle(Refresh refresh, AppSettings appSettings, [FromBody] RefreshPostModel post)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var f = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSecretProvider.GetSecret(appSettings.Auth.Secret))),
            ValidateIssuer = false,
            ValidateAudience = false,
            LifetimeValidator = CustomLifetimeValidator
        };
        
        var principal = tokenHandler.ValidateToken(post.Token, f, out var _);
        if (principal is null)
            return ResultHandler.Error(ErrorType.Auth, "Invalid refresh token");

        var userName = principal.Identity?.Name ?? "";
        var result = await refresh.Execute(new Refresh.Request(userName));

        if (result is { Success: true, Data: not null })
        {
            var token = TokenBuilder.CreateToken(result.Data, appSettings);
            var refreshToken = TokenBuilder.CreateRefreshToken(result.Data, appSettings);
            return ResultHandler.Success(new LoginModel(token, refreshToken));
        }

        return ResultHandler.Error(result.Error);
    }
    
    static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters param)
    {
        if (expires != null)
            return expires > DateTime.UtcNow;

        return false;
    }
}