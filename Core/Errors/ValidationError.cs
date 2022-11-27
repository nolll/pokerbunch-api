using System.Linq;

namespace Core.Errors;

public class ValidationError : UseCaseError
{
    public override ErrorType Type => ErrorType.Validation;

    public ValidationError(Validator validator) : this(string.Join(' ', validator.Errors.ToList()))
    {
    }

    public ValidationError(string message) : base(message)
    {
    }
}