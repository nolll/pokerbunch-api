using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class SiteUrlProvider
{
    private readonly string _host;

    public SiteUrlProvider(string host)
    {
        _host = host;
    }

    public Url AddUser => new AddUserUrl(_host);
    public Url JoinBunch(string bunchId, string code = null) => new JoinBunchUrl(_host, bunchId, code);
    public Url Login => new LoginUrl(_host);
    public Url ApiDocs => new DocsUrl(_host);
}