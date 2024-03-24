using System;

namespace Core.Errors;

public abstract class UseCaseError(string message)
{
    public abstract ErrorType Type { get; }
    public string Message { get; } = message;

    protected UseCaseError(Exception e) : this(e.Message)
    {
    }
}