using System;

namespace Core.Errors;

public class UnknownError(Exception e) : UseCaseError(e)
{
    public override ErrorType Type => ErrorType.Unknown;
}