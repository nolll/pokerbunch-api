using Api.Models.CommonModels;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Core.Errors;

namespace Api.Controllers;

[ApiController]
public abstract class BaseController(AppSettings appSettings) : Controller
{
    protected AppSettings AppSettings { get; } = appSettings;
    
    protected ObjectResult Model<T>(UseCaseResult<T> result, Func<object?> create) => result.Success
        ? Success(create())
        : Error(result.Error);

    protected ObjectResult Success(object? model) => Ok(model);

    protected ObjectResult Error(UseCaseError? error) => error is not null
        ? Error(error.Type, error.Message)
        : Error(ErrorType.Unknown, "Unknown error");

    protected ObjectResult Error(ErrorType errorType, string errorMessage)
    {
        var statusCode = GetStatusCode(errorType);
        var messageModel = new ErrorModel(errorMessage);

        return StatusCode((int)statusCode, messageModel);
    }

    private HttpStatusCode GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => HttpStatusCode.BadRequest,
        ErrorType.NotFound => HttpStatusCode.NotFound,
        ErrorType.Auth => HttpStatusCode.Unauthorized,
        ErrorType.AccessDenied => HttpStatusCode.Forbidden,
        ErrorType.Conflict => HttpStatusCode.Conflict,
        _ => HttpStatusCode.InternalServerError
    };
}