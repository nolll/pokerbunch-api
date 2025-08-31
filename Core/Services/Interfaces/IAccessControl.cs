using Core.Entities;

namespace Core.Services;

public interface IAccessControl
{
    CurrentBunch GetBunchById(string id);
    CurrentBunch GetBunchBySlug(string id);

    bool CanClearCache { get; }
    bool CanSendTestEmail { get; }
    bool CanSeeAppSettings { get; }
    bool CanListBunches { get; }
    bool CanListUsers { get; }
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
}