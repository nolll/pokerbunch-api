namespace Web.Common.Routes
{
    public static class ApiRoutes
    {
        public const string Home = "";
        public const string Token = "token";

        public const string BunchList = "bunch";
        public const string BunchGet = "bunch/{slug}";

        public const string PlayerList = "players/{slug}";
        public const string PlayerGet = "player/{id}";

        public const string RunningGame = "cashgame/running/{slug}";
        public const string Buyin = "cashgame/buyin/{slug}";
        public const string Report = "cashgame/report/{slug}";
        public const string Cashout = "cashgame/cashout/{slug}";
    }
}