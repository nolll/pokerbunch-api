using Core.Entities;
using Core.Services;

namespace Api.Auth;

public interface IAuth
{
    IPrincipal Principal { get; }
    
    CurrentBunch GetBunchById(string id);
    CurrentBunch GetBunchBySlug(string id);

    bool CanClearCache { get; }
    bool CanSendTestEmail { get; }
    bool CanSeeAppSettings { get; }
    bool CanListBunches { get; }
    bool CanListUsers { get; }
    string Id { get; }
    string UserName { get; }
    string DisplayName { get; }
    bool CanEditCashgame(string bunchId);
    bool CanDeleteCashgame(string bunchId);
    bool CanSeeCashgame(string bunchId);
    bool CanSeeLocation(string bunchId);
    bool CanAddLocation(string bunchId);
    bool CanEditBunch(string bunchId);
    bool CanListLocations(string bunchId);
    bool CanAddCashgame(string bunchId);
    bool CanGetBunch(string bunchId);
    bool CanSeePlayer(string bunchId);
    bool CanDeletePlayer(string bunchId);
    bool CanListPlayers(string bunchId);
    bool CanAddPlayer(string bunchId);
    bool CanAddEvent(string bunchId);
    bool CanEditCashgameAction(string bunchId);
    bool CanListEvents(string bunchId);
    bool CanInvitePlayer(string bunchId);
    bool CanSeeEventDetails(string bunchId);
    bool CanListCashgames(string bunchId);
    bool CanListPlayerCashgames(string bunchId);
    bool CanListEventCashgames(string bunchId);
    bool CanListCurrentGames(string bunchId);
    bool CanDeleteCheckpoint(string bunchId);
    bool CanEditCashgameActionsFor(string bunchId, string requestedPlayerId);
}