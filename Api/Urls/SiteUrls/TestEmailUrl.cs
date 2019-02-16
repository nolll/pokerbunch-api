namespace PokerBunch.Common.Urls.SiteUrls
{
    public class TestEmailUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "admin/sendemail";
    }
}