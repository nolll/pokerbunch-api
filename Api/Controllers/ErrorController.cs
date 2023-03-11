using System.Net;
using Api.Models.CommonModels;
using Api.Routes;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : Controller
{
    [Route(ApiRoutes.Error)]
    [HttpGet, HttpDelete, HttpPost, HttpPut]
    public MessageModel Index()
    {
        var exception = GetException();

        if(exception is not null)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return new ErrorModel(exception.Message);
        }

        return new ErrorModel(HttpContext.Response.StatusCode.ToString());
    }

    private Exception? GetException()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        return exceptionHandlerPathFeature?.Error;
    }
}