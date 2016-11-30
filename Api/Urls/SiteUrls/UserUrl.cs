namespace Api.Urls.SiteUrls
{
    public abstract class UserUrl : SiteUrl
    {
        protected UserUrl(string format, string userName)
            : base(RouteParams.ReplaceUserName(format, userName))
        {
        }
    }
}