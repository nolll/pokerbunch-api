using Api.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiUserUpdateUrl : ApiUrl
{
    private readonly string _userName;

    public ApiUserUpdateUrl(string userName)
    {
        _userName = userName;
    }

    protected override string Input => RouteParams.Replace(ApiRoutes.User.Update, RouteReplace.UserName(_userName));
}