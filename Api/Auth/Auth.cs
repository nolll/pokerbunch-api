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
    
    public CurrentBunch GetBunchById(string id) => UserBunches.FirstOrDefault(o => o.Id == id) ?? CreateEmptyBunch();
    public CurrentBunch GetBunchBySlug(string slug) => UserBunches.FirstOrDefault(o => o.Slug == slug) ?? CreateEmptyBunch();

    public bool CanClearCache => IsAdmin;
    public bool CanSendTestEmail => IsAdmin;
    public bool CanSeeAppSettings => IsAdmin;
    public bool CanListBunches => IsAdmin;
    public bool CanListUsers => IsAdmin;
    
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
    
    public string UserName
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
    public string Id => GetClaim(CustomClaimTypes.UserId) ?? "";
    public string DisplayName => GetClaim(CustomClaimTypes.UserDisplayName) ?? "";
    
    private TokenBunchModel[] UserTokenBunches
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

    private CurrentBunch[] UserBunches => UserTokenBunches.Select(ToCurrentBunch).ToArray();

    private static CurrentBunch ToCurrentBunch(TokenBunchModel b) => 
        new(b.Id, b.Slug, b.Name, b.PlayerId, b.PlayerName, Enum.Parse<Role>(b.Role, true));

    private string? GetClaim(string type) => HttpContextUser?.Claims.FirstOrDefault(o => o.Type == type)?.Value;

    private bool GetBoolClaim(string type)
    {
        var claim = GetClaim(type);
        return claim is not null && bool.Parse(claim);
    }
    
    private static CurrentBunch CreateEmptyBunch() => new("", "");
}