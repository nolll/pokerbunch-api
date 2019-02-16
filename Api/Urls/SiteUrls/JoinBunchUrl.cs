namespace PokerBunch.Common.Urls.SiteUrls
{
    public class JoinBunchUrl : SiteUrl
    {
        private readonly string _code;
        private readonly string _bunchId;

        public JoinBunchUrl(string bunchId, string code = null)
        {
            _bunchId = bunchId;
            _code = code;
        }

        protected override string Input
        {
            get
            {
                if (_code != null)
                    return RouteParams.Replace(RouteWithCode, RouteReplace.BunchId(_bunchId), RouteReplace.Code(_code));
                return RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
            }
        }

        public const string Route = "bunch/join/{bunchId}";
        public const string RouteWithCode = Route + "/{code}";
    }
}