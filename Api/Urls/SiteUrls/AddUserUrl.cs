using Api.Endpoints.Routes;

namespace Api.Urls.SiteUrls;

public class AddUserUrl : SiteUrl
{
    protected override string Input => SiteRoutes.AddUser;
}