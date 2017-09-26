using PokerBunch.Common.Urls.ApiUrls;
using PokerBunch.Common.Urls.SiteUrls;

namespace Api.Routes
{
    public class ApiRootUrl : ApiUrl
    {
        protected override string Input => Route;
        public const string Route = "";
    }

    public class ApiUserBunchesUrl : ApiUrl
    {
        protected override string Input => Route;
        public const string Route = "user/bunches";
    }

    public class ApiUserAppsUrl : ApiUrl
    {
        protected override string Input => Route;
        public const string Route = "user/apps";
    }

    public class ApiUsersUrl : ApiUrl
    {
        protected override string Input => Route;
        public const string Route = "users";
    }

    public class ApiBunchesUrl : ApiUrl
    {
        protected override string Input => Route;
        public const string Route = "bunches";
    }

    public class ApiBunchUrl : ApiUrl
    {
        private readonly string _slug;

        public ApiBunchUrl(string slug)
        {
            _slug = slug;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Slug(_slug));
        public const string Route = "bunches/{slug}";
    }

    public class ApiBunchEventsUrl : ApiUrl
    {
        private readonly string _slug;

        public ApiBunchEventsUrl(string slug)
        {
            _slug = slug;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Slug(_slug));
        public const string Route = "bunches/{slug}/events";
    }

    public static class ApiRoutes
    {
        public const string BunchJoin = "bunches/{slug}/join";
        public const string BunchPlayerList = "bunches/{slug}/players";
        public const string BunchPlayerAdd = BunchPlayerList;
        public const string BunchCurrentGames = "bunches/{slug}/cashgames/current";

        public const string PlayerGet = "players/{id}";
        public const string PlayerDelete = PlayerGet;
        public const string PlayerCashgameList = "players/{id}/cashgames";
        public const string PlayerInvite = "players/{id}/invite";

        public const string LocationGet = "locations/{id}";
        public const string LocationList = "bunches/{slug}/locations";
        public const string LocationAdd = LocationList;
        public const string LocationSave = LocationGet;
        public const string LocationDelete = LocationGet;

        public const string EventGet = "events/{id}";
        public const string EventCashgameList = "events/{id}/cashgames";

        public const string AppList = "apps";
        public const string AppAdd = AppList;
        public const string AppGet = "apps/{id}";
        public const string AppSave = "apps/{id}";
        public const string AppDelete = "apps/{id}";

        public const string Buyin = "cashgames/{id}/buyin";
        public const string Report = "cashgames/{id}/report";
        public const string Cashout = "cashgames/{id}/cashout";
        public const string EndCashgame = "cashgames/{id}/end";

        public const string CashgameYears = "bunches/{slug}/cashgames/years";

        public static class Admin
        {
            public const string SendEmail = "admin/sendemail";
            public const string ClearCache = "admin/clearcache";
        }
    }
}