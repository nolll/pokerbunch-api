using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiUserUpdateUrl(string userName) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.User.Update, RouteReplace.UserName(userName));
}