namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddLocationConfirmationUrl : SiteUrl
    {
        private readonly string _bunchId;

        public AddLocationConfirmationUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "location/created/{bunchId}";
    }
}