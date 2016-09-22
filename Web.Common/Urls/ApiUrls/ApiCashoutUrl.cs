using Web.Common.Routes;

namespace Web.Common.Urls.ApiUrls
{
    public class ApiCashoutUrl : SlugApiUrl
    {
        public ApiCashoutUrl(string slug)
            : base(ApiRoutes.Cashout, slug)
        {
        }
    }
}