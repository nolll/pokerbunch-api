using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiCashgameListUrl : ApiUrl
    {
        private readonly string _id;
        private readonly int? _year;

        public ApiCashgameListUrl(string id, int? year)
        {
            _id = id;
            _year = year;
        }

        protected override string Input
        {
            get {
                if (_year.HasValue)
                    return RouteParams.ReplaceYear(RouteParams.ReplaceId(ApiRoutes.CashgameListWithYear, _id), _year.Value);
                return RouteParams.ReplaceId(ApiRoutes.CashgameList, _id);
            }
        }
    }
}