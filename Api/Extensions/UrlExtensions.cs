using PokerBunch.Common.Urls.ApiUrls;

namespace Api.Extensions
{
    public static class UrlExtensions
    {
        public static string Absolute(this ApiUrl url)
        {
            return url.Absolute(Settings.ApiHost);
        }
    }
}