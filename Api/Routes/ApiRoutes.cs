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

    public class ApiBunchJoinUrl : ApiUrl
    {
        private readonly string _slug;

        public ApiBunchJoinUrl(string slug)
        {
            _slug = slug;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Slug(_slug));
        public const string Route = "bunches/{slug}/join";
    }

    public class ApiBunchPlayersUrl : ApiUrl
    {
        private readonly string _slug;

        public ApiBunchPlayersUrl(string slug)
        {
            _slug = slug;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Slug(_slug));
        public const string Route = "bunches/{slug}/players";
    }

    public class ApiPlayerUrl : ApiUrl
    {
        private readonly string _id;

        public ApiPlayerUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "players/{id}";
    }

    public class ApiBunchCashgamesCurrentUrl : ApiUrl
    {
        private readonly string _slug;

        public ApiBunchCashgamesCurrentUrl(string slug)
        {
            _slug = slug;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Slug(_slug));
        public const string Route = "bunches/{slug}/cashgames/current";
    }

    public class ApiPlayerInviteUrl : ApiUrl
    {
        private readonly string _id;

        public ApiPlayerInviteUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "players/{id}/invite";
    }

    public class ApiPlayerCashgamesUrl : ApiUrl
    {
        private readonly string _id;

        public ApiPlayerCashgamesUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "players/{id}/cashgames";
    }

    public class ApiBunchCashgameYearsUrl : ApiUrl
    {
        private readonly string _slug;

        public ApiBunchCashgameYearsUrl(string slug)
        {
            _slug = slug;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Slug(_slug));
        public const string Route = "bunches/{slug}/cashgames/years";
    }

    public class ApiLocationUrl : ApiUrl
    {
        private readonly string _id;

        public ApiLocationUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "locations/{id}";
    }

    public class ApiBunchLocationsUrl : ApiUrl
    {
        private readonly string _slug;

        public ApiBunchLocationsUrl(string slug)
        {
            _slug = slug;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Slug(_slug));
        public const string Route = "bunches/{slug}/locations";
    }

    public class ApiEventUrl : ApiUrl
    {
        private readonly string _id;

        public ApiEventUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "events/{id}";
    }

    public class ApiEventCashgamesUrl : ApiUrl
    {
        private readonly string _id;

        public ApiEventCashgamesUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "events/{id}/cashgames";
    }

    public class ApiAppUrl : ApiUrl
    {
        private readonly string _id;

        public ApiAppUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "apps/{id}";
    }

    public class ApiAppsUrl : ApiUrl
    {
        protected override string Input => Route;
        public const string Route = "apps";
    }

    public class ApiAdminSendEmailUrl : ApiUrl
    {
        protected override string Input => Route;
        public const string Route = "admin/sendemail";
    }

    public class ApiAdminClearCacheUrl : ApiUrl
    {
        protected override string Input => Route;
        public const string Route = "admin/clearcache";
    }

    public class ApiCashgameBuyinUrl : ApiUrl
    {
        private readonly string _id;

        public ApiCashgameBuyinUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "cashgames/{id}/buyin";
    }

    public class ApiCashgameReportUrl : ApiUrl
    {
        private readonly string _id;

        public ApiCashgameReportUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "cashgames/{id}/report";
    }

    public class ApiCashgameCashoutUrl : ApiUrl
    {
        private readonly string _id;

        public ApiCashgameCashoutUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "cashgames/{id}/cashout";
    }

    public class ApiCashgameEndUrl : ApiUrl
    {
        private readonly string _id;

        public ApiCashgameEndUrl(string id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.Id(_id));
        public const string Route = "cashgames/{id}/end";
    }
}