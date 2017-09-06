using Api.Routes;

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
                    return RouteParams.Replace(ApiRoutes.CashgameListWithYear, RouteParam.Slug(_id), RouteParam.Year(_year.Value));
                return RouteParams.Replace(ApiRoutes.CashgameList, RouteParam.Slug(_id));
            }
        }
    }
}