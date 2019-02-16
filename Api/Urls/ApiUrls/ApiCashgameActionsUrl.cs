using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiCashgameActionsUrl : ApiUrl
    {
        private readonly string _cashgameId;

        public ApiCashgameActionsUrl(string cashgameId)
        {
            _cashgameId = cashgameId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Action.List, RouteReplace.CashgameId(_cashgameId));
    }
}