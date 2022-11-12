namespace Api.Routes;

public static class ApiRoutes
{
    public const string Root = "";
    public const string Version = "version";
    public const string Settings = "settings";
    public const string Error = "error";
    public const string Swagger = "swagger";

    public static class Action
    {
        public const string Get = "cashgames/{cashgameId}/actions/{actionId}";
        public const string List = "cashgames/{cashgameId}/actions";
    }

    public static class Admin
    {
        public const string ClearCache = "admin/clearcache";
        public const string SendEmail = "admin/sendemail";
    }

    public static class Bunch
    {
        public const string Get = "bunches/{bunchId}";
        public const string Join = "bunches/{bunchId}/join";
        public const string List = "bunches";
        public const string Add = List;
        public const string ListForCurrentUser = "user/bunches";
    }

    public static class Cashgame
    {
        public const string Get = "cashgames/{cashgameId}";
        public const string ListByBunch = "bunches/{bunchId}/cashgames";
        public const string ListByBunchAndYear = "bunches/{bunchId}/cashgames/{year}";
        public const string ListCurrentByBunch = "bunches/{bunchId}/cashgames/current";
        public const string ListByEvent = "events/{eventId}/cashgames";
        public const string ListByPlayer = "players/{playerId}/cashgames";
        public const string YearsByBunch = "years/cashgames/{bunchId}";
    }
        
    public static class Event
    {
        public const string Get = "events/{eventId}";
        public const string ListByBunch = "bunches/{bunchId}/events";
    }

    public static class Location
    {
        public const string Get = "locations/{locationId}";
        public const string ListByBunch = "bunches/{bunchId}/locations";
    }
    
    public static class Player
    {
        public const string Get = "players/{playerId}";
        public const string Invite = "players/{playerId}/invite";
        public const string ListByBunch = "bunches/{bunchId}/players";
        public const string AddToBunch = ListByBunch;
    }

    public static class Profile
    {
        public const string Get = "user";
        public const string Password = "user/password";
    }

    public static class Auth
    {
        public const string Token = "token";
        public const string Login = "login";
    }

    public static class User
    {
        public const string Get = "users/{userName}";
        public const string List = "users";
        public const string Add = List;
    }
}