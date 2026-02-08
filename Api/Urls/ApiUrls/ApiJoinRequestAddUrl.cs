using Api.Endpoints.Routes;
using Api.Urls.SiteUrls;

namespace Api.Urls.ApiUrls;

public class ApiJoinRequestAddUrl(string bunchId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.JoinRequest.Add, RouteReplace.BunchId(bunchId));
}

public class ApiJoinRequestAcceptUrl(string joinRequestId) : ApiUrl
{
    protected override string Input => RouteParams.Replace(ApiRoutes.JoinRequest.Accept, RouteReplace.JoinRequestId(joinRequestId));
}