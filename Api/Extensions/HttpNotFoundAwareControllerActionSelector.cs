using System.Web.Http;
using System.Web.Http.Controllers;
using Core.Exceptions;

namespace Api.Extensions
{
    public class HttpNotFoundAwareControllerActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            try
            {
                return base.SelectAction(controllerContext);
            }
            catch (HttpResponseException)
            {
                throw new NotFoundException("Not found");
            }
        }
    }
}