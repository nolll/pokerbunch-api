using System;
using System.Linq;

namespace Core.Errors;

public class ValidationError(string message) : UseCaseError(message)
{
    public override ErrorType Type => ErrorType.Validation;

    public ValidationError(Validator validator) : this(string.Join(Environment.NewLine, validator.Errors.ToList()))
    {
    }
}