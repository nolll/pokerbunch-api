using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiCashgameUrl : IdApiUrl
    {
        public ApiCashgameUrl(int id)
            : base(ApiRoutes.CashgameGet, id)
        {
        }
    }
}