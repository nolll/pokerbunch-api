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
    public static string CreateToken(CommonLoginResult data, AppSettings appSettings)
    {
        var claims = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, data.UserName),
            new Claim(CustomClaimTypes.Version, "2"),
            new Claim(CustomClaimTypes.UserId, data.UserId),
            new Claim(CustomClaimTypes.UserDisplayName, data.DisplayName),
            new Claim(CustomClaimTypes.IsAdmin, data.IsAdmin.ToString().ToLower()),
            new Claim(CustomClaimTypes.Bunches, ToJson(data.BunchResults), JsonClaimValueTypes.JsonArray),
            new Claim(CustomClaimTypes.Seed, Guid.NewGuid().ToString())
        ]);

        return CreateToken(appSettings.Auth.Secret, TimeSpan.FromDays(365), claims);
    }

    public static string CreateRefreshToken(CommonLoginResult data, AppSettings appSettings)
    {
        var claims = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, data.UserName),
            new Claim(CustomClaimTypes.Version, "2"),
            new Claim(CustomClaimTypes.Seed, Guid.NewGuid().ToString())
        ]);

        return CreateToken(appSettings.Auth.Secret, TimeSpan.FromDays(730), claims);
    }

    private static string CreateToken(string secret, TimeSpan lifetime, ClaimsIdentity claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSecretProvider.GetSecret(secret));
        var symmetricKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.Add(lifetime),
            SigningCredentials = credentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string ToJson(List<LoginResultBunch> bunchResults)
    {
        var tokenBunches = bunchResults.Select(ToTokenBunch).ToArray();
        return JsonSerializer.Serialize(tokenBunches);
    }
    
    private static TokenBunchModel ToTokenBunch(LoginResultBunch b) => new(b.BunchId, b.BunchSlug, b.BunchName, b.PlayerId, b.PlayerName, b.Role.ToString().ToLower());
}