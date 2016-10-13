namespace Web.Common.Routes
{
    public static class ApiRoutes
    {
        public const string Home = "";
        public const string Token = "token";

        public const string UserBunchList = "user/bunches";

        public const string BunchList = "bunches";
        public const string BunchGet = "bunches/{slug}";

        public const string PlayerList = "player/list/{slug}";
        public const string PlayerGet = "player/get/{id}";

        public const string LocationList = "location/list/{slug}";
        public const string LocationGet = "location/get/{id}";
        public const string LocationAdd = "location/add";
        public const string LocationSave = "location/save";
        public const string LocationDelete = "location/delete/{id}";

        public const string RunningGame = "cashgame/running/{slug}";
        public const string Buyin = "cashgame/buyin/{slug}";
        public const string Report = "cashgame/report/{slug}";
        public const string Cashout = "cashgame/cashout/{slug}";
    }
}