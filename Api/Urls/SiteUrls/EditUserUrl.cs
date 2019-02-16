namespace PokerBunch.Common.Urls.SiteUrls
{
    public class EditUserUrl : SiteUrl
    {
        private readonly string _userName;

        public EditUserUrl(string userName)
        {
            _userName = userName;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.UserName(_userName));
        public const string Route = "user/edit/{userName}";
    }
}