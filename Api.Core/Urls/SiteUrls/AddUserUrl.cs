namespace Api.Urls.SiteUrls
{
    public class AddUserUrl : SiteUrl
    {
        protected override string Input => "user/add";

        public AddUserUrl(string host) : base(host)
        {
        }
    }
}