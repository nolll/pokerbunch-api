namespace PokerBunch.Common.Urls.SiteUrls
{
    public class AddEventConfirmationUrl : SiteUrl
    {
        private readonly string _bunchId;

        public AddEventConfirmationUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "event/created/{bunchId}";
    }
}