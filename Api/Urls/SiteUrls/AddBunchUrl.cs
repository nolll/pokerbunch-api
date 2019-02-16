namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddBunchUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "bunch/add";
    }
}