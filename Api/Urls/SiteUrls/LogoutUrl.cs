namespace PokerBunch.Common.Urls.SiteUrls
{
    public class LogoutUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "auth/logout";
    }
}