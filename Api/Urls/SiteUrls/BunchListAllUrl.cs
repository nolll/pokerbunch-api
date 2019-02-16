namespace PokerBunch.Common.Urls.SiteUrls
{
    public class BunchListAllUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "bunch/all";
    }
}