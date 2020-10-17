using Api.Routes;

namespace Api.Urls.ApiUrls
{
    public class ApiSwaggerUrl : ApiUrl
    {
        protected override string Input => ApiRoutes.Swagger;

        public ApiSwaggerUrl(string host) : base(host)
        {
        }
    }
}