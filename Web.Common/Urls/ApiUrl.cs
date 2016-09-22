namespace Web.Common.Urls
{
    public abstract class ApiUrl : Url
    {
        protected ApiUrl(string url)
            : base(url)
        {
        }

        public override UrlType Type
        {
            get { return UrlType.Api; }
        }
    }
}