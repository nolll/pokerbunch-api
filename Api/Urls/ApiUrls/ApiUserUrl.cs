using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiUserUrl(string userName) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.User.Get, RouteReplace.UserName(userName));
}