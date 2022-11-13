using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class SiteUrlProvider
{
    private readonly string _host;

    public SiteUrlProvider(string host)
    {
        _host = host;
    }

    public string AddUser => new AddUserUrl().Absolute(_host);
    public string JoinBunch(string bunchId, string code = null) => new JoinBunchUrl(bunchId, code).Absolute(_host);
    public string Login => new LoginUrl().Absolute(_host);
    public string ApiDocs => new DocsUrl().Absolute(_host);
}