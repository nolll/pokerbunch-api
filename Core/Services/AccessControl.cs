using Core.Entities;

namespace Core.Services;

public static class AccessControl
{
    public static bool CanClearCache(User currentUser) => IsAdmin(currentUser);
    public static bool CanSendTestEmail(User currentUser) => IsAdmin(currentUser);
    public static bool CanSeeAppSettings(User currentUser) => IsAdmin(currentUser);
    public static bool CanListBunches(User currentUser) => IsAdmin(currentUser);
    public static bool CanListUsers(User currentUser) => IsAdmin(currentUser);
    public static bool CanEditCashgameActionsFor(int requestedPlayerId, User currentUser, Player currentPlayer) =>
        IsAdmin(currentUser) || IsManager(currentPlayer) || IsRequestedPlayer(currentPlayer, requestedPlayerId);
    public static bool CanSeePlayer(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanSeeCashgame(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanSeeLocation(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanListLocations(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanSeeEventDetails(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanListCashgames(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanListPlayers(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanListPlayerCashgames(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanListEvents(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanListCashgameYears(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanListEventCashgames(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanAddPlayer(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsManager(currentPlayer);
    public static bool CanDeletePlayer(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsManager(currentPlayer);
    public static bool CanGetBunch(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanListCurrentGames(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanAddCashgame(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanAddEvent(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanAddLocation(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);

    private static bool IsRequestedPlayer(Player currentPlayer, int requestedPlayerId) => currentPlayer.Id == requestedPlayerId;
    private static bool IsPlayer(Player currentPlayer) => RoleHandler.IsInRole(currentPlayer, Role.Player);
    private static bool IsManager(Player currentPlayer) => RoleHandler.IsInRole(currentPlayer, Role.Manager);
    private static bool IsAdmin(User currentUser) => currentUser.IsAdmin;
}