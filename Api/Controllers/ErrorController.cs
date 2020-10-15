using System;
using System.Net;
using Api.Models.CommonModels;
using Api.Routes;
using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ErrorController : Controller
    {
        [Route(ApiRoutes.Error)]
        [HttpGet, HttpDelete, HttpPost, HttpPut]
        public ErrorModel Index()
        {
            var exception = GetException();
            var statusCode = GetStatusCode(exception);
            var message = GetMessage(exception);

            HttpContext.Response.StatusCode = (int)statusCode;
            return new ErrorModel(message);
        }

        private string GetMessage(Exception exception)
        {
            if (exception != null)
                return exception.Message;
            return "Not found";
        }

        private Exception GetException()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            return exceptionHandlerPathFeature?.Error;
        }

        private HttpStatusCode GetStatusCode(Exception ex)
        {
            var currentStatusCode = (HttpStatusCode)HttpContext.Response.StatusCode;
            if (currentStatusCode == HttpStatusCode.NotFound)
                return currentStatusCode;

            if (ex is NotFoundException)
                return HttpStatusCode.NotFound;

            if (ex is AccessDeniedException)
                return HttpStatusCode.Forbidden;

            if (ex is AuthException)
                return HttpStatusCode.Unauthorized;

            if (ex is ValidationException)
                return HttpStatusCode.BadRequest;

            if (ex is ConflictException)
                return HttpStatusCode.Conflict;

            return HttpStatusCode.InternalServerError;
        }
    }
}