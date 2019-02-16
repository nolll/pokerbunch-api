using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.SiteUrls;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiCashgameUrl : ApiUrl
    {
        private readonly string _cashgameId;

        public ApiCashgameUrl(string cashgameId)
        {
            _cashgameId = cashgameId;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.Cashgame.Get, RouteReplace.CashgameId(_cashgameId));
    }
}