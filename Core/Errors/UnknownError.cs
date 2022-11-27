using System;

namespace Core.Errors;

public class UnknownError : UseCaseError
{
    public UnknownError(Exception e) : base(e)
    {
    }

    public override ErrorType Type => ErrorType.Unknown;
}