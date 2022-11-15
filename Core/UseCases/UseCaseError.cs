using System;

namespace Core.UseCases;

public class UseCaseError
{
    public ErrorType Type { get; }
    public string Message { get; }

    public UseCaseError(ErrorType type, string message)
    {
        Type = type;
        Message = message;
    }

    public UseCaseError(Exception e)
    {
        Type = ErrorType.Unknown;
        Message = e.Message;
    }
}