namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ForgotPasswordConfirmationUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "user/passwordsent";
    }
}