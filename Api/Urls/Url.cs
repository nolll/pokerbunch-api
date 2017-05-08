namespace Api.Urls
{
    public enum UrlType
    {
        Site,
        Api
    }

    public abstract class Url
    {
        public string Relative { get; }
        public abstract UrlType Type { get; }

        protected Url(string url)
        {
            Relative = url != null ? string.Concat("/", url).ToLower() : string.Empty;
        }
        
        public override string ToString()
        {
            return Relative;
        }
    }

    public static class AbsoluteUrl
    {
        public static string Create(Url url, string siteHost, string apiHost)
        {
            var domainName = GetDomainName(url.Type, siteHost, apiHost);
            var relativeUrl = url.Relative;
            return $"https://{domainName}{relativeUrl}";
        }
        
        private static string GetDomainName(UrlType type, string siteHost, string apiHost)
        {
            if (type == UrlType.Api)
                return apiHost;
            return siteHost;
        }
    }
}