namespace Api.Urls.SiteUrls
{
    public abstract class SiteUrl : Url
    {
        protected SiteUrl(string url)
            : base(url)
        {
        }

        public override UrlType Type
        {
            get { return UrlType.Site; }
        }
    }
}