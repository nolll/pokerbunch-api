using Web.Common.Urls;

namespace Api.Extensions
{
    public static class ApiUrlExtensions
    {
        public static string GetAbsolute(this Url url)
        {
            return AbsoluteUrl.Create(url, ApiSettings.SiteHost, ApiSettings.ApiHost);
        }
    }
}