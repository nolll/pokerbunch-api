namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ChangePasswordConfirmationUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "user/changedpassword";
    }
}