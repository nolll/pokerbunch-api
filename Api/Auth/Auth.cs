using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Api.Models;
using Core;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Http;

namespace Api.Auth;

public class Auth(IHttpContextAccessor httpContextAccessor) : IAuth
{
    private static readonly DateTime TokenMinDate = DateTime.Parse("2025-08-31");

    private string UserName
    {
        get
        {
            if (HttpContextUser?.Identity is null)
                throw new PokerBunchException("Auth failed: No identity");

            if (!HttpContextUser.Identity.IsAuthenticated)
                throw new PokerBunchException("Auth failed: Not authenticated");
            
            if(HttpContextUser.Identity.Name is null)
                throw new PokerBunchException("Auth failed: No identity");

            if (IsTokenTooOld)
                throw new PokerBunchException("Token too old");

            return HttpContextUser.Identity.Name;
        }
    }

    private ClaimsPrincipal? HttpContextUser => httpContextAccessor.HttpContext?.User;

    private bool IsTokenTooOld => 
        DateTimeService.FromUnixTimeStamp(int.Parse(GetClaim(CustomClaimTypes.IssuedAt) ?? "0")) < TokenMinDate;

    private bool IsAdmin => GetBoolClaim(CustomClaimTypes.IsAdmin);
    private string UserId => GetClaim(CustomClaimTypes.UserId) ?? "";
    private string UserDisplayName => GetClaim(CustomClaimTypes.UserDisplayName) ?? "";
    
    private TokenBunchModel[] UserBunches
    {
        get
        {
            var value = GetClaim(CustomClaimTypes.Bunches);
            if (value is null or "")
                return [];

            if (value.StartsWith('{'))
                return [JsonSerializer.Deserialize<TokenBunchModel>(value)!];
            
            return JsonSerializer.Deserialize<TokenBunchModel[]>(value) ?? [];
        }
    }

    public IPrincipal Principal => new Principal(UserId, UserName, UserDisplayName, IsAdmin, UserBunches.Select(ToCurrentBunch).ToArray());

    private static CurrentBunch ToCurrentBunch(TokenBunchModel b)
    {
        return new CurrentBunch(b.Id, b.Slug, b.Name, b.PlayerId, b.PlayerName, Enum.Parse<Role>(b.Role, true));
    }

    private string? GetClaim(string type) => HttpContextUser?.Claims.FirstOrDefault(o => o.Type == type)?.Value;

    private bool GetBoolClaim(string type)
    {
        var claim = GetClaim(type);
        return claim is not null && bool.Parse(claim);
    }
}