namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ChangePasswordUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "user/changepassword";
    }
}