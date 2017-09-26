using PokerBunch.Common.Urls.ApiUrls;
using PokerBunch.Common.Urls.SiteUrls;

namespace Api.Extensions
{
    public static class UrlExtensions
    {
        public static string Absolute(this ApiUrl url)
        {
            return url.Absolute(Settings.ApiHost);
        }

        public static string Absolute(this SiteUrl url)
        {
            return url.Absolute(Settings.SiteHost);
        }
    }
}