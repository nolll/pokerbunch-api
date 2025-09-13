using Api.Endpoints.Routes;

namespace Api.Urls.ApiUrls;

public class ApiAdminSendEmailUrl : ApiUrl
{
    protected override string Input => ApiRoutes.Admin.SendEmail;
}