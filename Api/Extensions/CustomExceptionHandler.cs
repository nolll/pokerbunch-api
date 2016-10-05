using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Core.Exceptions;

namespace Api.Extensions
{
    public class CustomExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new ResponseMessageResult(CreateResponse(context));
        }

        private HttpResponseMessage CreateResponse(ExceptionHandlerContext context)
        {
            var e = context.Exception;

            if (e is NotFoundException)
                return CreateResponse(context, HttpStatusCode.NotFound, "Not found");

            if (e is AccessDeniedException)
                return CreateResponse(context, HttpStatusCode.Forbidden, "Forbidden");

            return CreateResponse(context, HttpStatusCode.InternalServerError, "Unhandled error");
        }

        private HttpResponseMessage CreateResponse(ExceptionHandlerContext context, HttpStatusCode code, string message)
        {
            return context.Request.CreateResponse(code, new { Message = message });
        }
    }
}