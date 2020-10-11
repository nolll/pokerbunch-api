using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiCashgameUrl : ApiUrl
    {
        private readonly string _cashgameId;

        public ApiCashgameUrl(string host, string cashgameId) : base(host)
        {
            _cashgameId = cashgameId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.Get, RouteReplace.CashgameId(_cashgameId));
    }

    public class UrlProvider
    {
        public ApiUrlProvider Api { get; }
        public SiteUrlProvider Site { get; }

        public UrlProvider(string apiHost, string siteHost)
        {
            Api = new ApiUrlProvider(apiHost);
            Site = new SiteUrlProvider(siteHost);
        }
    }

    public class ApiUrlProvider
    {
        private readonly string _host;

        public ApiUrlProvider(string host)
        {
            _host = host;
        }

        public Url BunchCashgames(string bunchId, int? year) => new ApiBunchCashgamesUrl(_host, bunchId, year);
        public Url Cashgame(string cashgameId) => new ApiCashgameUrl(_host, cashgameId);
        public Url User(string userName) => new ApiUserUrl(_host, userName);
        public Url UserProfile => new ApiUserProfileUrl(_host);
    }

    public class SiteUrlProvider
    {
        private readonly string _host;

        public SiteUrlProvider(string host)
        {
            _host = host;
        }

        public Url AddUser => new AddUserUrl(_host);
        public Url JoinBunch(string bunchId, string code = null) => new JoinBunchUrl(_host, bunchId, code);
        public Url Login => new LoginUrl(_host);
    }
}