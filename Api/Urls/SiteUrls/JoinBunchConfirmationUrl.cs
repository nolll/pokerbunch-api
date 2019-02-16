namespace PokerBunch.Common.Urls.SiteUrls
{
    public class JoinBunchConfirmationUrl : SiteUrl
    {
        private readonly string _bunchId;

        public JoinBunchConfirmationUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "bunch/joined/{bunchId}";
    }
}