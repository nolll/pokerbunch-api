namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ErrorOtherUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "error/servererror";
    }
}