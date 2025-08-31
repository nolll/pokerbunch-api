using System.Linq;
using Core.Entities;

namespace Core.Services;

public class AccessControl(CurrentUser currentUser, TokenBunch[] userBunches) : IAccessControl
{
    public bool CanClearCache => IsAdmin();
    public bool CanSendTestEmail => IsAdmin();
    public bool CanSeeAppSettings => IsAdmin();
    
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
    public bool CanListBunches => IsAdmin();
    
    public bool CanAddPlayer(string bunchId) => IsManager(bunchId);
    public bool CanSeePlayer(string bunchId) => IsPlayer(bunchId);
    public bool CanListPlayers(string bunchId) => IsPlayer(bunchId);
    public bool CanDeletePlayer(string bunchId) => IsManager(bunchId);
    public bool CanInvitePlayer(string bunchId) => IsManager(bunchId);

    public bool CanAddEvent(string bunchId) => IsPlayer(bunchId);
    public bool CanListEvents(string bunchId) => IsPlayer(bunchId);
    public bool CanSeeEventDetails(string bunchId) => IsPlayer(bunchId);
    
    public bool CanListUsers => IsAdmin();

    private bool IsAdmin() => currentUser.IsAdmin;
    private bool IsManager(string bunchId) => IsInRole(bunchId, Role.Manager);
    private bool IsPlayer(string bunchId) => IsInRole(bunchId, Role.Player);
    private bool IsRequestedPlayer(string bunchId, string requestedPlayerId) => GetPlayerId(bunchId) == requestedPlayerId;
    private bool IsInRole(string bunchId, Role role) => RoleHandler.IsInRole(GetRole(bunchId), role);
    private Role GetRole(string bunchId) => GetBunchById(bunchId).Role;
    private string GetPlayerId(string bunchId) => GetBunchById(bunchId).PlayerId;

    public CurrentBunch GetBunchById(string id)
    {
        var b = userBunches.First(o => o.Id == id);
        return new CurrentBunch(b.Id, b.Slug, b.Name, b.PlayerId, b.PlayerName, b.Role);
    }
    
    public CurrentBunch GetBunchBySlug(string slug)
    {
        var b = userBunches.First(o => o.Slug == slug);
        return new CurrentBunch(b.Id, b.Slug, b.Name, b.PlayerId, b.PlayerName, b.Role);
    }
}