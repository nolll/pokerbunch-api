namespace Api.Routes
{
    public static class ApiRoutes
    {
        public const string Home = "";
        public const string Token = "token";

        public const string UserBunchList = "user/bunches";
        public const string UserAppList = "user/apps";
        public const string UserProfile = "user";

        public const string UserGet = "userbyname/{name}";

        public const string BunchList = "bunches";
        public const string BunchGet = "bunches/{slug}";
        public const string LocationList = "bunches/{slug}/locations";
        public const string EventList = "bunches/{slug}/events";
        public const string PlayerList = "bunches/{slug}/players";
        public const string CurrentGames = "bunches/{slug}/cashgames/current";

        public const string PlayerGet = "players/{id}";

        public const string LocationGet = "locations/{id}";
        public const string LocationAdd = "locations";
        public const string LocationSave = "locations/{id}";
        public const string LocationDelete = "locations/{id}";

        public const string AppList = "apps";
        public const string AppGet = "apps/{id}";
        public const string AppAdd = "apps";
        public const string AppSave = "apps/{id}";
        public const string AppDelete = "apps/{id}";

        public const string Buyin = "cashgame/buyin/{slug}";
        public const string Report = "cashgame/report/{slug}";
        public const string Cashout = "cashgame/cashout/{slug}";

        public const string Cashgames = "bunches/{slug}/cashgames/{year:int?}";
        public const string CashgameItem = "cashgames/{id}";

        public static class Admin
        {
            public const string SendEmail = "admin/sendemail";
            public const string ClearCache = "admin/clearcache";
        }
    }
}