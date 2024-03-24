using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class SiteUrlProvider(string host)
{
    public string AddUser => new AddUserUrl().Absolute(host);
    public string JoinBunch(string bunchId, string? code = null) => new JoinBunchUrl(bunchId, code).Absolute(host);
    public string Login => new LoginUrl().Absolute(host);
}