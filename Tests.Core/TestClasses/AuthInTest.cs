using Core.Entities;
using Core.Services;

namespace Tests.Core.TestClasses;

public class AuthInTest(
    string? id = null,
    string? userName = null,
    bool canClearCache = false,
    bool canSendTestEmail = false,
    bool canListBunches = false,
    bool canEditCashgame = false,
    bool canDeleteCashgame = false,
    bool canSeeCashgame = false,
    bool canSeeLocation = false,
    bool canListUsers = false,
    bool canViewFullUserData = false,
    bool canAddLocation = false,
    bool canEditBunch = false,
    bool canListLocations = false,
    bool canAddCashgame = false,
    bool canGetBunch = false,
    bool canSeePlayer = false,
    bool canDeletePlayer = false,
    bool canListPlayers = false,
    bool canAddPlayer = false,
    bool canAddEvent = false,
    bool canEditCashgameAction = false,
    bool canListEvents = false,
    bool canInvitePlayer = false,
    bool canSeeEventDetails = false,
    bool canListCashgames = false,
    bool canListPlayerCashgames = false,
    bool canListEventCashgames = false,
    bool canListCurrentGames = false,
    bool canDeleteCheckpoint = false,
    bool canEditCashgameActionsFor = false,
    UserBunch? userBunch = null) : IAuth
{
    public string Id => id ?? "";
    public string UserName => userName ?? "";
    public bool CanClearCache => canClearCache;
    public bool CanSendTestEmail => canSendTestEmail;
    public bool CanListBunches => canListBunches;
    public bool CanListUsers => canListUsers;
    public bool CanViewFullUserData => canViewFullUserData;
    public bool CanEditCashgame(string slug) => canEditCashgame;
    public bool CanDeleteCashgame(string slug) => canDeleteCashgame;
    public bool CanSeeCashgame(string slug) => canSeeCashgame;
    public bool CanSeeLocation(string slug) => canSeeLocation;
    public bool CanAddLocation(string slug) => canAddLocation;
    public bool CanEditBunch(string slug) => canEditBunch;
    public bool CanListLocations(string slug) => canListLocations;
    public bool CanAddCashgame(string slug) => canAddCashgame;
    public bool CanGetBunch(string slug) => canGetBunch;
    public bool CanSeePlayer(string slug) => canSeePlayer;
    public bool CanDeletePlayer(string slug) => canDeletePlayer;
    public bool CanListPlayers(string slug) => canListPlayers;
    public bool CanAddPlayer(string slug) => canAddPlayer;
    public bool CanAddEvent(string slug) => canAddEvent;
    public bool CanEditCashgameAction(string slug) => canEditCashgameAction;
    public bool CanListEvents(string slug) => canListEvents;
    public bool CanInvitePlayer(string slug) => canInvitePlayer;
    public bool CanSeeEventDetails(string slug) => canSeeEventDetails;
    public bool CanListCashgames(string slug) => canListCashgames;
    public bool CanListPlayerCashgames(string slug) => canListPlayerCashgames;
    public bool CanListEventCashgames(string slug) => canListEventCashgames;
    public bool CanListCurrentGames(string slug) => canListCurrentGames;
    public bool CanDeleteCheckpoint(string slug) => canDeleteCheckpoint;
    public bool CanEditCashgameActionsFor(string slug, string requestedPlayerId) => canEditCashgameActionsFor;
    
    public UserBunch GetBunch(string slug) => userBunch!;
}