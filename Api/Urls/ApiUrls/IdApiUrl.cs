using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls
{
    public abstract class IdApiUrl : ApiUrl
    {
        protected IdApiUrl(string format, int id)
            : base(RouteParams.ReplaceId(format, id))
        {
        }
    }
}