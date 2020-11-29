using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions
{
    public static class RequestExtensions
    {
        public static async Task<string> BodyAsString(this HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}