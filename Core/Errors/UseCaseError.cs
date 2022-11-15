using System;

namespace Core.Errors;

public abstract class UseCaseError
{
    public abstract ErrorType Type { get; }
    public string Message { get; }

    protected UseCaseError(string message)
    {
        Message = message;
    }

    protected UseCaseError(Exception e)
    {
        Message = e.Message;
    }
}