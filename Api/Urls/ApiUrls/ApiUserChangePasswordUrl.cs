using Api.Routes;

namespace Api.Urls.ApiUrls;

public class ApiUserChangePasswordUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Profile.Password;

    public ApiUserChangePasswordUrl(string host) : base(host)
    {
    }
}