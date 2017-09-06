using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public class ApiCashgameUrl : ApiUrl
    {
        private readonly int _id;

        public ApiCashgameUrl(int id)
        {
            _id = id;
        }

        protected override string Input => RouteParams.Replace(ApiRoutes.CashgameGet, RouteParam.Id(_id));
    }
}