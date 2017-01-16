using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace Api.Controllers
{
    public class NoContentResult<T> : NegotiatedContentResult<T>
    {
        public NoContentResult(T content, ApiController controller) : base(HttpStatusCode.NoContent, content, controller)
        {
        }
    }
}