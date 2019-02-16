namespace PokerBunch.Common.Urls.SiteUrls
{
    public class ApiDocsPlayersUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "apidocs/players";
    }
}