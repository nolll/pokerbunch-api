namespace Api.Urls.ApiUrls;

public class UrlProvider(string apiHost, string siteHost)
{
    public ApiUrlProvider Api { get; } = new(apiHost);
    public SiteUrlProvider Site { get; } = new(siteHost);
}