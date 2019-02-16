namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ForgotPasswordUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "user/forgotpassword";
    }
}