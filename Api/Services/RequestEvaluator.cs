using System.Net.Http;
using System.Web;
using Web.Common;

namespace Api.Services
{
    public static class RequestEvaluator
    {
        public static bool IsTestEnvironment(HttpRequestMessage request)
        {
            return IsTestEnvironment(GetHostName(request));
        }

        public static bool IsTestEnvironment(HttpRequestBase request)
        {
            return IsTestEnvironment(GetHostName(request));
        }

        private static bool IsTestEnvironment(string hostName)
        {
            return Environment.IsDev(hostName) || Environment.IsTest(hostName) || Environment.IsStage(hostName);
        }

        private static string GetHostName(HttpRequestMessage request)
        {
            return request.RequestUri.Host;
        }

        private static string GetHostName(HttpRequestBase request)
        {
            return request.Url?.Host ?? string.Empty;
        }
    }
}