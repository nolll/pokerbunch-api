using Core.Entities;

namespace Core.Services;

public static class RoleHandler
{
    public static bool IsInRole(Role testedRole, Role requiredRole) => testedRole >= requiredRole;
}