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

    public CurrentBunch GetBunchById(string id) => Principal.GetBunchById(id);
    public CurrentBunch GetBunchBySlug(string id) => Principal.GetBunchBySlug(id);

    public bool CanClearCache => Principal.CanClearCache;
    public bool CanSendTestEmail => Principal.CanSendTestEmail;
    public bool CanSeeAppSettings => Principal.CanSeeAppSettings;
    public bool CanListBunches => Principal.CanListBunches;
    public bool CanListUsers => Principal.CanClearCache;
    public string Id => Principal.Id;
    public string DisplayName => Principal.DisplayName;
    public bool CanEditCashgame(string bunchId) => Principal.CanEditCashgame(bunchId);
    public bool CanDeleteCashgame(string bunchId) => Principal.CanDeleteCashgame(bunchId);
    public bool CanSeeCashgame(string bunchId) => Principal.CanSeeCashgame(bunchId);
    public bool CanSeeLocation(string bunchId) => Principal.CanSeeLocation(bunchId);
    public bool CanAddLocation(string bunchId) => Principal.CanAddLocation(bunchId);
    public bool CanEditBunch(string bunchId) => Principal.CanEditBunch(bunchId);
    public bool CanListLocations(string bunchId) => Principal.CanListLocations(bunchId);
    public bool CanAddCashgame(string bunchId) => Principal.CanAddCashgame(bunchId);
    public bool CanGetBunch(string bunchId) => Principal.CanGetBunch(bunchId);
    public bool CanSeePlayer(string bunchId) => Principal.CanSeePlayer(bunchId);
    public bool CanDeletePlayer(string bunchId) => Principal.CanDeletePlayer(bunchId);
    public bool CanListPlayers(string bunchId) => Principal.CanListPlayers(bunchId);
    public bool CanAddPlayer(string bunchId) => Principal.CanAddPlayer(bunchId);
    public bool CanAddEvent(string bunchId) => Principal.CanAddEvent(bunchId);
    public bool CanEditCashgameAction(string bunchId) => Principal.CanEditCashgameAction(bunchId);
    public bool CanListEvents(string bunchId) => Principal.CanListEvents(bunchId);
    public bool CanInvitePlayer(string bunchId) => Principal.CanInvitePlayer(bunchId);
    public bool CanSeeEventDetails(string bunchId) => Principal.CanSeeEventDetails(bunchId);
    public bool CanListCashgames(string bunchId) => Principal.CanListCashgames(bunchId);
    public bool CanListPlayerCashgames(string bunchId) => Principal.CanListPlayerCashgames(bunchId);
    public bool CanListEventCashgames(string bunchId) => Principal.CanListEventCashgames(bunchId);
    public bool CanListCurrentGames(string bunchId) => Principal.CanListCurrentGames(bunchId);
    public bool CanDeleteCheckpoint(string bunchId) => Principal.CanDeleteCheckpoint(bunchId);
    public bool CanEditCashgameActionsFor(string bunchId, string requestedPlayerId) => Principal.CanEditCashgameActionsFor(bunchId, requestedPlayerId);

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