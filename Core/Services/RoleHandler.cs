using Core.Entities;

namespace Core.Services;

public static class RoleHandler
{
    public static bool IsInRole(User user, Player? player, Role role)
    {
        return user.IsAdmin || player != null && player.IsInRole(role);
    }

    public static bool IsInRole(Player? player, Role role)
    {
        return player != null && player.IsInRole(role);
    }
    
    public static bool IsInRole(Role testedRole, Role requiredRole) => testedRole >= requiredRole;
}