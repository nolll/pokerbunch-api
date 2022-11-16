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
    public static bool CanAddPlayer(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsManager(currentPlayer);
    public static bool CanDeletePlayer(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsManager(currentPlayer);
    public static bool CanGetBunch(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);
    public static bool CanListCurrentGames(User currentUser, Player currentPlayer) => IsAdmin(currentUser) || IsPlayer(currentPlayer);

    private static bool IsRequestedPlayer(Player currentPlayer, int requestedPlayerId) => currentPlayer.Id == requestedPlayerId;
    private static bool IsPlayer(Player currentPlayer) => RoleHandler.IsInRole(currentPlayer, Role.Player);
    private static bool IsManager(Player currentPlayer) => RoleHandler.IsInRole(currentPlayer, Role.Manager);
    private static bool IsAdmin(User currentUser) => currentUser.IsAdmin;
}