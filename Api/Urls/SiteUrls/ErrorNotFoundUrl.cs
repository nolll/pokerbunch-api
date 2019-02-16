namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ErrorNotFoundUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "error/notfound";
    }
}