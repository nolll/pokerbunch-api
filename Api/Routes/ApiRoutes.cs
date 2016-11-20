namespace Api.Routes
{
    public static class ApiRoutes
    {
        public const string Home = "";
        public const string Token = "token";

        public const string UserBunchList = "user/bunches";
        public const string UserAppList = "user/apps";
        public const string UserProfile = "user";

        public const string UserGet = "users/{name}";

        public const string BunchList = "bunches";
        public const string BunchGet = "bunches/{slug}";

        public const string PlayerList = "player/list/{slug}";
        //public const string PlayerList = "bunches/{slug}/players";
        public const string PlayerGet = "player/get/{id}";
        //public const string PlayerGet = "players/{id}";

        public const string LocationList = "location/list/{slug}";
        //public const string LocationList = "bunches/{slug}/locations";
        public const string LocationGet = "location/get/{id}";
        //public const string LocationGet = "locations/{id}";
        public const string LocationAdd = "location/add";
        //public const string LocationAdd = "locations";
        public const string LocationSave = "location/save";
        //public const string LocationSave = "locations/{id}";
        public const string LocationDelete = "location/delete/{id}";
        //public const string LocationDelete = "locations/{id}";

        public const string AppList = "apps";
        public const string AppGet = "apps/{id}";
        public const string AppAdd = "apps";
        public const string AppSave = "apps/{id}";
        public const string AppDelete = "apps/{id}";

        public const string RunningGame = "cashgame/running/{slug}";
        public const string Buyin = "cashgame/buyin/{slug}";
        public const string Report = "cashgame/report/{slug}";
        public const string Cashout = "cashgame/cashout/{slug}";
    }
}