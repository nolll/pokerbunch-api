using System.Web.Http.ExceptionHandling;
using PokerBunch.Common.Urls.ApiUrls;

namespace Api.Extensions
{
    public class CustomErrorLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var exception = context.Exception;
            // Write your custom logging code here
        }
    }

    public static class UrlExtensions
    {
        public static string Absolute(this ApiUrl url)
        {
            return url.Absolute(Settings.ApiHost);
        }
    }
}