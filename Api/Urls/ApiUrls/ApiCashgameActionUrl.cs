using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiCashgameActionUrl : ApiUrl
    {
        private readonly string _cashgameId;
        private readonly string _actionId;

        public ApiCashgameActionUrl(string cashgameId, string actionId)
        {
            _cashgameId = cashgameId;
            _actionId = actionId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Action.Get, RouteReplace.CashgameId(_cashgameId), RouteReplace.ActionId(_actionId));
    }
}