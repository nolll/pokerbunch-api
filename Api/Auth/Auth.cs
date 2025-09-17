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
    private static readonly DateTime TokenMinDate = DateTime.Parse("2025-08-31");
    
    private readonly bool _isAdmin;
    private readonly UserBunch[] _userBunches = [];

    public string Id { get; } = "";
    public string UserName { get; } = "";
    public string DisplayName { get; } = "";

    public Auth(IHttpContextAccessor httpContextAccessor)
    {
        var principal = httpContextAccessor.HttpContext?.User;
        if (!IsValid(principal))
            return;
        
        if (IsTokenTooOld(principal!))
            throw new PokerBunchException("Token too old");
        
        _isAdmin = GetBoolClaim(principal, CustomClaimTypes.IsAdmin);
        Id = GetClaim(principal, CustomClaimTypes.UserId) ?? "";
        DisplayName = GetClaim(principal, CustomClaimTypes.UserDisplayName) ?? "";
        UserName = principal!.Identity?.Name ?? "";
        _userBunches = GetUserTokenBunches(principal!).Select(ToUserBunch).ToArray();
    }
    
    public UserBunch GetBunchById(string id) => _userBunches.FirstOrDefault(o => o.Id == id) ?? CreateEmptyBunch();
    public UserBunch GetBunchBySlug(string slug) => _userBunches.FirstOrDefault(o => o.Slug == slug) ?? CreateEmptyBunch();

    public bool CanClearCache => _isAdmin;
    public bool CanSendTestEmail => _isAdmin;
    public bool CanSeeAppSettings => _isAdmin;
    public bool CanListBunches => _isAdmin;
    public bool CanListUsers => _isAdmin;
    public bool CanViewFullUserData => _isAdmin;
    
    public bool CanAddCashgame(string bunchId) => IsPlayer(bunchId);
    public bool CanEditCashgame(string bunchId) => IsManager(bunchId);
    public bool CanDeleteCashgame(string bunchId) => IsManager(bunchId);
    public bool CanSeeCashgame(string bunchId) => IsPlayer(bunchId);
    public bool CanListCashgames(string bunchId) => IsPlayer(bunchId);
    public bool CanListPlayerCashgames(string bunchId) => IsPlayer(bunchId);
    public bool CanListEventCashgames(string bunchId) => IsPlayer(bunchId);
    public bool CanListCurrentGames(string bunchId) => IsPlayer(bunchId);
    
    public bool CanEditCashgameAction(string bunchId) => IsManager(bunchId);
    public bool CanDeleteCheckpoint(string bunchId) => IsManager(bunchId);
    public bool CanEditCashgameActionsFor(string bunchId, string requestedPlayerId) =>
        IsManager(bunchId) || IsRequestedPlayer(bunchId, requestedPlayerId);

    public bool CanSeeLocation(string bunchId) => IsPlayer(bunchId);
    public bool CanAddLocation(string bunchId) => IsPlayer(bunchId);
    public bool CanListLocations(string bunchId) => IsPlayer(bunchId);

    public bool CanGetBunch(string bunchId) => IsPlayer(bunchId);
    public bool CanEditBunch(string bunchId) => IsManager(bunchId);
    
    public bool CanAddPlayer(string bunchId) => IsManager(bunchId);
    public bool CanSeePlayer(string bunchId) => IsPlayer(bunchId);
    public bool CanListPlayers(string bunchId) => IsPlayer(bunchId);
    public bool CanDeletePlayer(string bunchId) => IsManager(bunchId);
    public bool CanInvitePlayer(string bunchId) => IsManager(bunchId);

    public bool CanAddEvent(string bunchId) => IsPlayer(bunchId);
    public bool CanListEvents(string bunchId) => IsPlayer(bunchId);
    public bool CanSeeEventDetails(string bunchId) => IsPlayer(bunchId);

    private bool IsManager(string bunchId) => IsInRole(bunchId, Role.Manager);
    private bool IsPlayer(string bunchId) => IsInRole(bunchId, Role.Player);
    private bool IsRequestedPlayer(string bunchId, string requestedPlayerId) => GetPlayerId(bunchId) == requestedPlayerId;
    private bool IsInRole(string bunchId, Role role) => RoleHandler.IsInRole(GetRole(bunchId), role);
    private Role GetRole(string bunchId) => GetBunchById(bunchId).Role;
    private string GetPlayerId(string bunchId) => GetBunchById(bunchId).PlayerId;

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
        DateTimeService.FromUnixTimeStamp(int.Parse(GetClaim(principal, CustomClaimTypes.IssuedAt) ?? "0"));

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
        new(b.Id, b.Slug, b.Name, b.PlayerId, b.PlayerName, Enum.Parse<Role>(b.Role, true));
    
    private static string? GetClaim(ClaimsPrincipal? principal, string type) => principal?.Claims.FirstOrDefault(o => o.Type == type)?.Value;

    private static bool GetBoolClaim(ClaimsPrincipal? principal, string type)
    {
        var claim = GetClaim(principal, type);
        return claim is not null && bool.Parse(claim);
    }
    
    private static UserBunch CreateEmptyBunch() => new("", "");
}