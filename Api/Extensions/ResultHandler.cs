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

    public static IResult Error(ErrorType errorType, string errorMessage)
    {
        var messageModel = new ErrorModel(errorMessage);

        return errorType switch
        {
            ErrorType.Validation => Results.BadRequest(messageModel),
            ErrorType.NotFound => Results.NotFound(messageModel),
            ErrorType.Auth => Results.Unauthorized(),
            ErrorType.AccessDenied => Results.StatusCode(StatusCodes.Status403Forbidden),
            ErrorType.Conflict => Results.Conflict(messageModel),
            _ => Results.InternalServerError(messageModel)
        };
    }
}