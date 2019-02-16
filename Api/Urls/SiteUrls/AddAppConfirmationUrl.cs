namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddAppConfirmationUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "apps/added";
    }
}