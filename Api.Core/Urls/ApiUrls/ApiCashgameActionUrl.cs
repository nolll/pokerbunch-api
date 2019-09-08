using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiCashgameActionUrl : ApiUrl
    {
        private readonly string _cashgameId;
        private readonly string _actionId;

        public ApiCashgameActionUrl(string host, string cashgameId, string actionId) : base(host)
        {
            _cashgameId = cashgameId;
            _actionId = actionId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Action.Get, RouteReplace.CashgameId(_cashgameId), RouteReplace.ActionId(_actionId));
    }
}