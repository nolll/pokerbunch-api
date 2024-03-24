namespace Core.Errors;

public class ConflictError(string message) : UseCaseError(message)
{
    public override ErrorType Type => ErrorType.Conflict;
}