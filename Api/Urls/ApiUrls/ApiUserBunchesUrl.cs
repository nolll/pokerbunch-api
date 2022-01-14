using Api.Routes;

namespace Api.Urls.ApiUrls;

public class ApiUserBunchesUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Bunch.ListForCurrentUser;

    public ApiUserBunchesUrl(string host) : base(host)
    {
    }
}