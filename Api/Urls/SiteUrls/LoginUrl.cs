namespace PokerBunch.Common.Urls.SiteUrls
{
    public class LoginUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "auth/login";
    }
}