using Api.Routes;

namespace Api.Urls.SiteUrls;

public class LoginUrl : SiteUrl
{
    protected override string Input => SiteRoutes.Login;
}