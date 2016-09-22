using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Web.Common.Services;

namespace Api.Extensions
{
    public class EnsureHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.Request;

            if (RequestEvaluator.IsTestEnvironment(request))
                return;

            if (request.RequestUri.Scheme == Uri.UriSchemeHttps)
                return;

            actionContext.Response = GetResponse(request);
        }

        private HttpResponseMessage GetResponse(HttpRequestMessage request)
        {
            var uriBuilder = GetUriBuilder(request.RequestUri);
            var body = GetBody(uriBuilder);
            if (request.Method.Equals(HttpMethod.Get) || request.Method.Equals(HttpMethod.Head))
            {
                var response = request.CreateResponse(HttpStatusCode.Found);
                response.Headers.Location = uriBuilder.Uri;
                if (request.Method.Equals(HttpMethod.Get))
                {
                    response.Content = new StringContent(body, Encoding.UTF8, "text/html");
                }
                return response;
            }
            else
            {
                var response = request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent(body, Encoding.UTF8, "text/html");
                return response;
            }
        }

        private UriBuilder GetUriBuilder(Uri requestUri)
        {
            return new UriBuilder(requestUri)
            {
                Scheme = Uri.UriSchemeHttps,
                Port = 443
            };
        }

        private string GetBody(UriBuilder uriBuilder)
        {
            var absoluteUrl = uriBuilder.Uri.AbsoluteUri;
            return $"<p>The resource can be found at <a href=\"{absoluteUrl}\">{absoluteUrl}</a>.</p>";
        }
    }
}