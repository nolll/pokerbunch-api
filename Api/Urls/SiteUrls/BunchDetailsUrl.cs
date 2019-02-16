namespace PokerBunch.Common.Urls.SiteUrls
{
    public class BunchDetailsUrl : SiteUrl
    {
        private readonly string _bunchId;

        public BunchDetailsUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "bunch/details/{bunchId}";
    }
}