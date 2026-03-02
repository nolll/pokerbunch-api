using Api.Urls.SiteUrls;
using Core.Services.Interfaces;

namespace Api.Urls.ApiUrls;

public class SiteUrlProvider(string host) : ISiteUrlProvider
{
    public string Login() => new LoginUrl().Absolute(host);
    public string JoinRequestList(string bunchId) => new JoinRequestListUrl(bunchId).Absolute(host);
}