using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Api.Services;
using Core.Exceptions;

namespace Api.Extensions
{
    public class CustomExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var isFriendlyErrorMessagesEnabled = !new Environment(context.Request.RequestUri.Host).IsDevMode;
            if (isFriendlyErrorMessagesEnabled)
                context.Result = new ResponseMessageResult(CreateResponse(context));
        }

        private HttpResponseMessage CreateResponse(ExceptionHandlerContext context)
        {
            var e = context.Exception;

            if (e is NotFoundException)
                return CreateResponse(context, HttpStatusCode.NotFound, e.Message);

            if (e is AccessDeniedException)
                return CreateResponse(context, HttpStatusCode.Forbidden, "Forbidden");

            if (e is ConflictException)
                return CreateResponse(context, HttpStatusCode.Conflict, e.Message);

            if (e is ValidationException)
            {
                var validationException = e as ValidationException;
                var message = validationException.Messages.First() ?? validationException.Message;
                return CreateResponse(context, HttpStatusCode.BadRequest, message);
            }

            return CreateResponse(context, HttpStatusCode.InternalServerError, "Unhandled error.");
        }

        private HttpResponseMessage CreateResponse(ExceptionHandlerContext context, HttpStatusCode code, string message)
        {
            return context.Request.CreateResponse(code, new { Message = message });
        }
    }
}