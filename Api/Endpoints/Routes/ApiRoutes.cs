namespace Api.Endpoints.Routes;

public static class ApiRoutes
{
    public const string Error = "/error";
    public const string Root = "/";
    public const string Swagger = "/swagger/index.html";
    public const string Version = "/version";

    public static class Action
    {
        public const string Add = "/cashgames/{cashgameId}/actions";
        public const string Delete = Update;
        public const string Update = "/cashgames/{cashgameId}/actions/{actionId}";
    }

    public static class Admin
    {
        public const string ClearCache = "/admin/clearcache";
        public const string SendEmail = "/admin/sendemail";
    }

    public static class Bunch
    {
        public const string Add = List;
        public const string Get = "/bunches/{bunchId}";
        public const string Join = "/bunches/{bunchId}/join";
        public const string List = "/bunches";
        public const string ListForCurrentUser = "/user/bunches";
        public const string Update = Get;
    }

    public static class Cashgame
    {
        public const string Add = ListByBunch;
        public const string Delete = Get;
        public const string Get = "/cashgames/{cashgameId}";
        public const string Update = Get;
        public const string ListByBunch = "/bunches/{bunchId}/cashgames";
        public const string ListByBunchAndYear = "/bunches/{bunchId}/cashgames/{year}";
        public const string ListByEvent = "/events/{eventId}/cashgames";
        public const string ListByPlayer = "/players/{playerId}/cashgames";
        public const string ListCurrentByBunch = "/bunches/{bunchId}/cashgames/current";
    }

    public static class Event
    {
        public const string Add = ListByBunch;
        public const string Get = "/events/{eventId}";
        public const string ListByBunch = "/bunches/{bunchId}/events";
    }

    public static class Location
    {
        public const string Add = ListByBunch;
        public const string Get = "/locations/{locationId}";
        public const string ListByBunch = "/bunches/{bunchId}/locations";
    }

    public static class Player
    {
        public const string Add = ListByBunch;
        public const string Delete = Get;
        public const string Get = "/players/{playerId}";
        public const string Invite = "/players/{playerId}/invite";
        public const string ListByBunch = "/bunches/{bunchId}/players";
    }

    public static class Profile
    {
        public const string ChangePassword = "/user/password";
        public const string Get = "/user";
        public const string ResetPassword = ChangePassword;
    }

    public static class Auth
    {
        public const string Login = "/login";
        public const string Refresh = "/refresh";
    }

    public static class User
    {
        public const string Add = List;
        public const string Get = "/users/{userName}";
        public const string List = "/users";
        public const string Update = Get;
    }
}
