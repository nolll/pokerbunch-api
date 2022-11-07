using Api.Routes;

namespace Api.Urls.ApiUrls;

public class ApiTokenUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Auth.Token;

    public ApiTokenUrl(string host) : base(host)
    {
    }
}