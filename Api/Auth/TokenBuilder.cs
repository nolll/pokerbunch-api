using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Api.Models;
using Api.Settings;
using Core.UseCases;
using Microsoft.IdentityModel.Tokens;

namespace Api.Auth;

public static class TokenBuilder
{
    public static string CreateToken(Login.Result data, AppSettings appSettings)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSecretProvider.GetSecret(appSettings.Auth.Secret));
        var symmetricKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, data.UserName),
            new Claim(CustomClaimTypes.Version, "2"),
            new Claim(CustomClaimTypes.UserId, data.UserId),
            new Claim(CustomClaimTypes.UserDisplayName, data.DisplayName),
            new Claim(CustomClaimTypes.IsAdmin, data.IsAdmin.ToString().ToLower()),
            new Claim(CustomClaimTypes.Bunches, ToJson(data.BunchResults), JsonClaimValueTypes.JsonArray)
        ]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials = credentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static string CreateRefreshToken(Login.Result data, AppSettings appSettings)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSecretProvider.GetSecret(appSettings.Auth.Secret));
        var symmetricKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, data.UserName),
            new Claim(CustomClaimTypes.Version, "2")
        ]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddYears(2),
            SigningCredentials = credentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string ToJson(List<Login.ResultBunch> bunchResults)
    {
        var tokenBunches = bunchResults.Select(ToTokenBunch).ToArray();
        return JsonSerializer.Serialize(tokenBunches);
    }
    
    private static TokenBunchModel ToTokenBunch(Login.ResultBunch b) => new(b.BunchId, b.BunchSlug, b.BunchName, b.PlayerId, b.PlayerName, b.Role.ToString().ToLower());
}