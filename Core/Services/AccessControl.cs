using Core.Entities;

namespace Core.Services;

public static class AccessControl
{
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
}