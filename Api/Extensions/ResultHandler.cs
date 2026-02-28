using System.Net;
using Api.Models.CommonModels;
using Core.Errors;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions;

public static class ResultHandler
{
    public static IResult Model<T>(UseCaseResult<T> result, Func<object?> create) => result.Success
        ? Success(create())
        : Error(result.Error);

    public static IResult Success(object? model) => Results.Ok(model);

    public static IResult Error(UseCaseError? error) => error is not null
        ? Error(error.Type, error.Message)
        : Error(ErrorType.Unknown, "Unknown error");

    public static IResult Error(ErrorType errorType, string errorMessage) => 
        Results.Json(new ErrorModel(errorMessage), statusCode: (int)GetStatusCode(errorType));

    private static HttpStatusCode GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => HttpStatusCode.BadRequest,
        ErrorType.NotFound => HttpStatusCode.NotFound,
        ErrorType.Auth => HttpStatusCode.Unauthorized,
        ErrorType.AccessDenied => HttpStatusCode.Forbidden,
        ErrorType.Conflict => HttpStatusCode.Conflict,
        _ => HttpStatusCode.InternalServerError
    };
}