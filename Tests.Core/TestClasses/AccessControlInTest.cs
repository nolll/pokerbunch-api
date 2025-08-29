using Core.Entities;
using Core.Services;

namespace Tests.Core.TestClasses;

public class AccessControlInTest(
    bool canClearCache = false,
    bool canSendTestEmail = false,
    bool canSeeAppSettings = false,
    bool canListBunches = false,
    bool canEditCashgame = false,
    bool canDeleteCashgame = false,
    bool canSeeCashgame = false,
    bool canSeeLocation = false,
    bool canListUsers = false,
    bool canAddLocation = false,
    bool canEditBunch = false,
    bool canListLocations = false,
    bool canAddCashgame = false,
    CurrentBunch? currentBunch = null) : IAccessControl
{
    public bool CanClearCache => canClearCache;
    public bool CanSendTestEmail => canSendTestEmail;
    public bool CanSeeAppSettings => canSeeAppSettings;
    public bool CanListBunches => canListBunches;
    public bool CanListUsers => canListUsers;
    public bool CanEditCashgame(string bunchId) => canEditCashgame;
    public bool CanDeleteCashgame(string bunchId) => canDeleteCashgame;
    public bool CanSeeCashgame(string bunchId) => canSeeCashgame;
    public bool CanSeeLocation(string bunchId) => canSeeLocation;
    public bool CanAddLocation(string bunchId) => canAddLocation;
    public bool CanEditBunch(string bunchId) => canEditBunch;
    public bool CanListLocations(string bunchId) => canListLocations;
    public bool CanAddCashgame(string bunchId) => canAddCashgame;

    public CurrentBunch GetBunchById(string id) => currentBunch!;
    public CurrentBunch GetBunchBySlug(string slug) => currentBunch!;
}