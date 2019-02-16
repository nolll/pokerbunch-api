namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddUserConfirmationUrl : SiteUrl
    {
        public const string Route = "user/created";
        protected override string Input => Route;
    }
}