namespace Api.Urls.ApiUrls;

public class UrlProvider
{
    public ApiUrlProvider Api { get; }
    public SiteUrlProvider Site { get; }

    public UrlProvider(string apiHost, string siteHost)
    {
        Api = new ApiUrlProvider(apiHost);
        Site = new SiteUrlProvider(siteHost);
    }
}