using System.Web;
using PokerBunch.Common.Urls.ApiUrls;
using PokerBunch.Common.Urls.SiteUrls;

namespace Api.Extensions
{
    public static class UrlExtensions
    {
        public static string Absolute(this ApiUrl url)
        {
            return url.Absolute(ApiHost);
        }

        public static string Absolute(this SiteUrl url)
        {
            return url.Absolute(Settings.SiteHost);
        }

        private static string ApiHost => HttpContext.Current?.Request.Url.Host ?? Settings.ApiHost;
    }
}