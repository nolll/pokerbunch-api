namespace Api.Urls
{
    public abstract class Url
    {
        protected abstract UrlType Type { get; }
        protected abstract string Input { get; }
        public string Relative => Input != null ? string.Concat("/", Input).ToLower() : string.Empty;
        public override string ToString() => Relative;

        public string Absolute => GetAbsolute(Settings.SiteHost, Settings.ApiHost);

        public string GetAbsolute(string siteHost, string apiHost)
        {
            var domainName = GetDomainName(Type, siteHost, apiHost);
            return $"https://{domainName}{Relative}";
        }

        private static string GetDomainName(UrlType type, string siteHost, string apiHost)
        {
            return type == UrlType.Api ? apiHost : siteHost;
        }
    }
}