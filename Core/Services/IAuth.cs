using Core.Entities;

namespace Core.Services;

public interface IAuth
{
    UserBunch GetBunch(string id);

    bool CanClearCache { get; }
    bool CanSendTestEmail { get; }
    bool CanSeeAppSettings { get; }
    bool CanListBunches { get; }
    bool CanListUsers { get; }
    bool CanViewFullUserData { get; }
    string Id { get; }
    string UserName { get; }
    string DisplayName { get; }
    bool CanEditCashgame(string slug);
    bool CanDeleteCashgame(string slug);
    bool CanSeeCashgame(string slug);
    bool CanSeeLocation(string slug);
    bool CanAddLocation(string slug);
    bool CanEditBunch(string slug);
    bool CanListLocations(string slug);
    bool CanAddCashgame(string slug);
    bool CanGetBunch(string slug);
    bool CanSeePlayer(string slug);
    bool CanDeletePlayer(string slug);
    bool CanListPlayers(string slug);
    bool CanAddPlayer(string slug);
    bool CanAddEvent(string slug);
    bool CanEditCashgameAction(string slug);
    bool CanListEvents(string slug);
    bool CanInvitePlayer(string slug);
    bool CanSeeEventDetails(string slug);
    bool CanListCashgames(string slug);
    bool CanListPlayerCashgames(string slug);
    bool CanListEventCashgames(string slug);
    bool CanListCurrentGames(string slug);
    bool CanDeleteCheckpoint(string slug);
    bool CanEditCashgameActionsFor(string slug, string requestedPlayerId);
}