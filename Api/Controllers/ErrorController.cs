using System;
using System.Net;
using Api.Models.AdminModels;
using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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

            return HttpStatusCode.InternalServerError;
        }
    }
}