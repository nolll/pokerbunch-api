using Api.Routes;

namespace Api.Urls.ApiUrls;

public class ApiTokenUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Token.Get;

    public ApiTokenUrl(string host) : base(host)
    {
    }
}