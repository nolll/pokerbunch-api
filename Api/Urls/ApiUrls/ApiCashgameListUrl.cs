using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiCashgameListUrl : ApiUrl
    {
        public ApiCashgameListUrl(int id)
            : base(RouteParams.ReplaceId(ApiRoutes.CashgameList, id))
        {
        }

        public ApiCashgameListUrl(string id, int? year)
            : base(Replace(id, year))
        {
        }

        private static string Replace(string id, int? year)
        {
            if (year.HasValue)
                return RouteParams.ReplaceYear(RouteParams.ReplaceId(ApiRoutes.CashgameListWithYear, id), year.Value);
            return RouteParams.ReplaceId(ApiRoutes.CashgameList, id);
        }
    }
}