namespace Core.Errors;

public class ConflictError : UseCaseError
{
    public override ErrorType Type => ErrorType.Conflict;

    public ConflictError(string message) : base(message)
    {
    }
}