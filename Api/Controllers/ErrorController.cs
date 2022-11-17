using System;
using System.Net;
using Api.Models.CommonModels;
using Api.Routes;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ErrorController : Controller
{
    [Route(ApiRoutes.Error)]
    [HttpGet, HttpDelete, HttpPost, HttpPut]
    public MessageModel Index()
    {
        var exception = GetException();
        var message = GetMessage(exception);

        HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return new ErrorModel(message);
    }

    private string GetMessage(Exception exception)
    {
        return exception != null 
            ? exception.Message 
            : "Not found";
    }

    private Exception GetException()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        return exceptionHandlerPathFeature?.Error;
    }
}