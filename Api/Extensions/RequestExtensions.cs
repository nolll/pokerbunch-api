using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions
{
    public static class RequestExtensions
    {
        public static string BodyAsString(this HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                return reader.ReadToEnd();
            }
        }
    }
}