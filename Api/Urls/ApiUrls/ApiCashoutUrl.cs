using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiCashoutUrl : SlugApiUrl
    {
        public ApiCashoutUrl(string slug)
            : base(ApiRoutes.Cashout, slug)
        {
        }
    }
}