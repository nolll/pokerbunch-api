using Api.Routes;

namespace Api.Urls.SiteUrls;

public class AddUserUrl : SiteUrl
{
    protected override string Input => SiteRoutes.AddUser;

    public AddUserUrl(string host) : base(host)
    {
    }
}