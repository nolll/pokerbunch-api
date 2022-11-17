using Core.Entities;
using Core.Exceptions;

namespace Core.Services;

public static class RequireRole
{
    public static void Manager(User user, Player player)
    {
        Require(user, player, Role.Manager);
    }
    
    private static void Require(User user, Player player, Role role)
    {
        if (!RoleHandler.IsInRole(user, player, role))
            throw new AccessDeniedException();
    }
}