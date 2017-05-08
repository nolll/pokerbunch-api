namespace Api.Urls
{
    public abstract class ApiUrl : Url
    {
        protected ApiUrl(string url)
            : base(url)
        {
        }

        public override UrlType Type => UrlType.Api;
    }
}