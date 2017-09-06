namespace Api.Urls.SiteUrls
{
    public abstract class SiteUrl : Url
    {
        protected override string Host => Settings.SiteHost;
    }
}