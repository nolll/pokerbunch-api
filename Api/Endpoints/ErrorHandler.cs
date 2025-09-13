using System.Net;
using Api.Extensions;
using Api.Models.CommonModels;
using Core.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints;

public static class ErrorHandler
{
    public static IResult Handle(IHttpContextAccessor httpContextAccessor)
    {
        var exceptionHandlerPathFeature = httpContextAccessor.HttpContext?.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        if (exception is null) 
            return ResultHandler.Success(new ErrorModel(httpContextAccessor.HttpContext?.Response.StatusCode.ToString() ?? ""));
        
        if(httpContextAccessor.HttpContext is not null)
            httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        return ResultHandler.Error(ErrorType.Unknown, exception.Message);        
    }
}