namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddAppUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "apps/add";
    }
}