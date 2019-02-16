namespace PokerBunch.Common.Urls.SiteUrls
{
    public class TopListUrl : SiteUrl
    {
        private readonly string _bunchId;
        private readonly int? _year;

        public TopListUrl(string bunchId, int? year = null)
        {
            _bunchId = bunchId;
            _year = year;
        }

        protected override string Input
        {
            get
            {
                if (_year.HasValue)
                    return RouteParams.Replace(RouteWithYear, RouteReplace.BunchId(_bunchId), RouteReplace.Year(_year.Value));
                return RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
            }
        }

        public const string Route = "cashgame/toplist/{bunchId}";
        public const string RouteWithYear = Route + "/{year}";
    }
}