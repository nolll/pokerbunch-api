using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiBunchCashgamesUrl(string bunchId, int? year = null) : ApiUrl
{
    protected override string Input => year.HasValue
        ? RouteParams.Replace(ApiRoutes.Cashgame.ListByBunchAndYear, RouteReplace.BunchId(bunchId), RouteReplace.Year(year.Value))
        : RouteParams.Replace(ApiRoutes.Cashgame.ListByBunch, RouteReplace.BunchId(bunchId));
}