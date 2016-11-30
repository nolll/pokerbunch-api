using Api.Urls;

namespace Api.Extensions
{
    public static class ApiUrlExtensions
    {
        public static string GetAbsolute(this Url url)
        {
            return AbsoluteUrl.Create(url, Settings.SiteHost, Settings.ApiHost);
        }
    }
}