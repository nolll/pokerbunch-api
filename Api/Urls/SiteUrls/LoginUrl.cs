namespace Api.Urls.SiteUrls
{
    public class LoginUrl : SiteUrl
    {
        protected override string Input => "auth/login";

        public LoginUrl(string host) : base(host)
        {
        }
    }
}