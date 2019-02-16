namespace PokerBunch.Common.Urls.SiteUrls
{
    public class EditBunchUrl : SiteUrl
    {
        private readonly string _bunchId;

        public EditBunchUrl(string bunchId)
        {
            _bunchId = bunchId;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.BunchId(_bunchId));
        public const string Route = "bunch/edit/{bunchId}";
    }
}