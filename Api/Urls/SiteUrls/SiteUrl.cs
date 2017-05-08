namespace Api.Urls.SiteUrls
{
    public abstract class SiteUrl : Url
    {
        protected override UrlType Type => UrlType.Site;
    }
}