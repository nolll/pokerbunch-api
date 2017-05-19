namespace Api.Routes
{
    public static class ApiRoutes
    {
        public const string Home = "";
        public const string Token = "token";

        public const string UserBunchList = "user/bunches";
        public const string UserAppList = "user/apps";
        public const string UserProfile = "user";

        public const string UserList = "users";
        public const string UserGet = "users/{userName}";
        public const string UserUpdate = UserGet;

        public const string BunchList = "bunches";
        public const string BunchAdd = BunchList;
        public const string BunchGet = "bunches/{slug}";
        public const string BunchUpdate = BunchGet;
        public const string EventList = "bunches/{slug}/events";
        public const string EventAdd = EventList;
        public const string PlayerList = "bunches/{slug}/players";
        public const string CurrentGames = "bunches/{slug}/cashgames/current";

        public const string PlayerGet = "players/{id}";

        public const string LocationGet = "locations/{id}";
        public const string LocationList = "bunches/{slug}/locations";
        public const string LocationAdd = LocationList;
        public const string LocationSave = LocationGet;
        public const string LocationDelete = LocationGet;

        public const string EventGet = "events/{id}";

        public const string AppList = "apps";
        public const string AppAdd = AppList;
        public const string AppGet = "apps/{id}";
        public const string AppSave = "apps/{id}";
        public const string AppDelete = "apps/{id}";

        public const string Buyin = "cashgame/buyin/{slug}";
        public const string Report = "cashgame/report/{slug}";
        public const string Cashout = "cashgame/cashout/{slug}";

        public const string CashgameList = "bunches/{id}/cashgames";
        public const string CashgameAdd = CashgameList;
        public const string CashgameListWithYear = "bunches/{id}/cashgames/{year}";
        public const string CashgameYears = "bunches/{id}/cashgames/years";
        public const string CashgameGet = "cashgames/{id}";
        public const string CashgameUpdate = CashgameGet;
        public const string CashgameDelete = CashgameGet;

        public static class Admin
        {
            public const string SendEmail = "admin/sendemail";
            public const string ClearCache = "admin/clearcache";
        }
    }
}