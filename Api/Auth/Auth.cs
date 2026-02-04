using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Api.Models;
using Core;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Http;

namespace Api.Auth;

public class Auth : IAuth
{
    private static readonly DateTime TokenMinDate = DateTime.Parse("2025-08-31T00:00:00Z").ToUniversalTime();
    
    private readonly bool _isAdmin;
    private readonly UserBunch[] _userBunches = [];

    public string Id { get; } = "";
    public string UserName { get; } = "";

    public Auth(IHttpContextAccessor httpContextAccessor)
    {
        var principal = httpContextAccessor.HttpContext?.User;
        if (!IsValid(principal))
            return;
        
        if (IsTokenTooOld(principal!))
            throw new PokerBunchException("Token too old");
        
        _isAdmin = GetBoolClaim(principal, CustomClaimTypes.IsAdmin);
        Id = GetClaim(principal, CustomClaimTypes.UserId) ?? "";
        UserName = principal!.Identity?.Name ?? "";
        _userBunches = GetUserTokenBunches(principal!).Select(ToUserBunch).ToArray();
    }
    
    public UserBunch GetBunch(string slug) => _userBunches.FirstOrDefault(o => o.Slug == slug) ?? CreateEmptyBunch();

    public bool CanClearCache => _isAdmin;
    public bool CanSendTestEmail => _isAdmin;
    public bool CanListBunches => _isAdmin;
    public bool CanListUsers => _isAdmin;
    public bool CanViewFullUserData => _isAdmin;
    
    public bool CanAddCashgame(string slug) => IsPlayer(slug);
    public bool CanEditCashgame(string slug) => IsManager(slug);
    public bool CanDeleteCashgame(string slug) => IsManager(slug);
    public bool CanSeeCashgame(string slug) => IsPlayer(slug);
    public bool CanListCashgames(string slug) => IsPlayer(slug);
    public bool CanListPlayerCashgames(string slug) => IsPlayer(slug);
    public bool CanListEventCashgames(string slug) => IsPlayer(slug);
    public bool CanListCurrentGames(string slug) => IsPlayer(slug);
    
    public bool CanEditCashgameAction(string slug) => IsManager(slug);
    public bool CanDeleteCheckpoint(string slug) => IsManager(slug);
    public bool CanEditCashgameActionsFor(string slug, string requestedPlayerId) =>
        IsManager(slug) || IsRequestedPlayer(slug, requestedPlayerId);

    public bool CanSeeLocation(string slug) => IsPlayer(slug);
    public bool CanAddLocation(string slug) => IsPlayer(slug);
    public bool CanListLocations(string slug) => IsPlayer(slug);

    public bool CanGetBunch(string slug) => IsPlayer(slug);
    public bool CanEditBunch(string slug) => IsManager(slug);
    
    public bool CanAddPlayer(string slug) => IsManager(slug);
    public bool CanSeePlayer(string slug) => IsPlayer(slug);
    public bool CanListPlayers(string slug) => IsPlayer(slug);
    public bool CanDeletePlayer(string slug) => IsManager(slug);
    public bool CanInvitePlayer(string slug) => IsManager(slug);

    public bool CanAddEvent(string slug) => IsPlayer(slug);
    public bool CanListEvents(string slug) => IsPlayer(slug);
    public bool CanSeeEventDetails(string slug) => IsPlayer(slug);
    
    private bool IsManager(string slug) => IsInRole(slug, Role.Manager);
    private bool IsPlayer(string slug) => IsInRole(slug, Role.Player);
    private bool IsRequestedPlayer(string slug, string requestedPlayerId) => GetPlayerId(slug) == requestedPlayerId;
    private bool IsInRole(string slug, Role role) => RoleHandler.IsInRole(GetRole(slug), role);
    private Role GetRole(string slug) => GetBunch(slug).Role;
    private string GetPlayerId(string slug) => GetBunch(slug).PlayerId;

    private static bool IsValid(ClaimsPrincipal? principal)
    {
        if (principal?.Identity is null)
            return false;

        if (!principal.Identity.IsAuthenticated)
            return false;
            
        return principal.Identity.Name is not null;
    }
    
    private static bool IsTokenTooOld(ClaimsPrincipal principal) => GetIssuedAt(principal) < TokenMinDate;

    private static DateTime GetIssuedAt(ClaimsPrincipal principal) => 
        DateTimeService.UtcFromUnixTimeStamp(int.Parse(GetClaim(principal, CustomClaimTypes.IssuedAt) ?? "0"));

    private static TokenBunchModel[] GetUserTokenBunches(ClaimsPrincipal principal)
    {
        var value = GetClaim(principal, CustomClaimTypes.Bunches);
        if (value is null or "")
            return [];

        return value.StartsWith('{')
            ? [JsonSerializer.Deserialize<TokenBunchModel>(value)!]
            : JsonSerializer.Deserialize<TokenBunchModel[]>(value) ?? [];
    }

    private static UserBunch ToUserBunch(TokenBunchModel b) => 
        new(b.Slug, b.Name, b.PlayerId, b.PlayerName, Enum.Parse<Role>(b.Role, true));
    
    private static string? GetClaim(ClaimsPrincipal? principal, string type) => principal?.Claims.FirstOrDefault(o => o.Type == type)?.Value;

    private static bool GetBoolClaim(ClaimsPrincipal? principal, string type)
    {
        var claim = GetClaim(principal, type);
        return claim is not null && bool.Parse(claim);
    }
    
    private static UserBunch CreateEmptyBunch() => new("", "");
}