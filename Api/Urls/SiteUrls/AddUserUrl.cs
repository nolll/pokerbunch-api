using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class AddUserUrl : SiteUrl
    {
        protected override string Input => WebRoutes.User.Add;
    }
}