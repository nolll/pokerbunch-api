using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class SiteUrlProvider(string host)
{
    public string Login => new LoginUrl().Absolute(host);
}