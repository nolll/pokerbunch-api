using PokerBunch.Common.Routes;

namespace PokerBunch.Common.Urls.ApiUrls
{
    public class ApiBunchesUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Bunch.List;
    }
}