using Core.Entities;
using Core.Exceptions;

namespace Core.Services;

public static class RequireRole
{
    public static void Player(User user, Player player)
    {
        Require(user, player, Role.Player);
    }

    public static void Manager(User user, Player player)
    {
        Require(user, player, Role.Manager);
    }

    public static void Admin(User user)
    {
        if (!user.IsAdmin)
            throw new AccessDeniedException();
    }

    public static void Me(User user, int userId)
    {
        if (!user.IsAdmin && userId != user.Id)
            throw new AccessDeniedException();
    }

    public static void Me(User user, Player player, int playerId)
    {
        if (!user.IsAdmin && playerId != player.Id)
            throw new AccessDeniedException();
    }

    private static void Require(User user, Player player, Role role)
    {
        if (!RoleHandler.IsInRole(user, player, role))
            throw new AccessDeniedException();
    }
}