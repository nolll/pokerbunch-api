namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddBunchConfirmationUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "bunch/created";
    }
}