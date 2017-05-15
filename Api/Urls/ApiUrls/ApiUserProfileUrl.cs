using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiUserProfileUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.UserProfile;
    }
}