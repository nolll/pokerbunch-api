namespace Api.Endpoints.Routes;

public static class SiteRoutes
{
    public const string AddUser = "/user/add";
    public const string JoinBunch = "/bunches/{bunchId}/join";
    public const string JoinBunchWithCode = "/bunches/{bunchId}/join/{code}";
    public const string Login = "/auth/login";
}