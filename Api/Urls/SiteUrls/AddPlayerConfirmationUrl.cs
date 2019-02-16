namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddPlayerConfirmationUrl : SiteUrl
    {
        private readonly string _bunchId;

        public AddPlayerConfirmationUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "player/created/{bunchId}";
    }
}