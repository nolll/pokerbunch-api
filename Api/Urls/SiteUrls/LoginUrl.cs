using Api.Routes;

namespace Api.Urls.SiteUrls;

public class LoginUrl : SiteUrl
{
    protected override string Input => SiteRoutes.Login;

    public LoginUrl(string host) : base(host)
    {
    }
}