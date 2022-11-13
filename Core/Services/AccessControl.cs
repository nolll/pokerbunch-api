using Core.Entities;

namespace Core.Services;

public static class AccessControl
{
    public static bool CanClearCache(User currentUser)
    {
        if (currentUser.IsAdmin)
            return true;

        return false;
    }

    public static bool CanSendTestEmail(User currentUser)
    {
        if (currentUser.IsAdmin)
            return true;

        return false;
    }

    public static bool CanSeeAppSettings(User currentUser)
    {
        if (currentUser.IsAdmin)
            return true;

        return false;
    }

    public static bool CanListBunches(User currentUser)
    {
        if (currentUser.IsAdmin)
            return true;

        return false;
    }

    public static bool CanListUsers(User currentUser)
    {
        if (currentUser.IsAdmin)
            return true;

        return false;
    }

    public static bool CanEditCashgameActionsFor(int requestPlayerId, User currentUser, Player currentPlayer)
    {
        if (currentUser.IsAdmin)
            return true;

        if (RoleHandler.IsInRole(currentPlayer, Role.Manager))
            return true;

        if (currentPlayer.Id == requestPlayerId)
            return true;

        return false;
    }

    public static bool CanSeePlayer(User currentUser, Player currentPlayer)
    {
        if (currentUser.IsAdmin)
            return true;

        if (RoleHandler.IsInRole(currentPlayer, Role.Player))
            return true;

        return false;
    }

    public static bool CanDeletePlayer(User currentUser, Player currentPlayer)
    {
        if (currentUser.IsAdmin)
            return true;

        if (RoleHandler.IsInRole(currentPlayer, Role.Manager))
            return true;

        return false;
    }
}