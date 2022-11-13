using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiBunchCashgamesUrl : ApiUrl
{
    private readonly string _bunchId;
    private readonly int? _year;

    public ApiBunchCashgamesUrl(string bunchId, int? year = null)
    {
        _bunchId = bunchId;
        _year = year;
    }

    protected override string Input => _year.HasValue
        ? RouteParams.Replace(ApiRoutes.Cashgame.ListByBunchAndYear, RouteReplace.BunchId(_bunchId), RouteReplace.Year(_year.Value))
        : RouteParams.Replace(ApiRoutes.Cashgame.ListByBunch, RouteReplace.BunchId(_bunchId));
}