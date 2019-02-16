namespace PokerBunch.Common.Urls.SiteUrls
{
    public class UserListUrl : SiteUrl
    {
        protected override string Input => Route;
        public const string Route = "user/list";
    }
}