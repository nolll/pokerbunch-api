using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiAdminSendEmailUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Admin.SendEmail;
    }
}