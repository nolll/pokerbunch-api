using Core.Entities;

namespace Core.Services;

public interface IAccessControl
{
    CurrentBunch GetBunch(string id);

    bool CanClearCache { get; }
    bool CanSendTestEmail { get; }
    bool CanSeeAppSettings { get; }
    bool CanListBunches { get; }
    bool CanEditCashgame(string bunchId);
    bool CanDeleteCashgame(string bunchId);
    bool CanSeeCashgame(string bunchId);
    bool CanSeeLocation(string bunchId);
}