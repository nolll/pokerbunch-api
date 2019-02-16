using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiTokenUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Token.Get;
    }
}