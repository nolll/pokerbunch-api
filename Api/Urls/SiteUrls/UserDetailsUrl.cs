namespace PokerBunch.Common.Urls.SiteUrls
{
    public class UserDetailsUrl : SiteUrl
    {
        private readonly string _userName;

        public UserDetailsUrl(string userName)
        {
            _userName = userName;
        }

        protected override string Input => RouteParams.Replace(Route, RouteReplace.UserName(_userName));
        public const string Route = "user/details/{userName}";
    }
}