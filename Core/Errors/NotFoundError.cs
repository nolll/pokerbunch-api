namespace Core.Errors;

public abstract class NotFoundError(string message) : UseCaseError(message)
{
    public override ErrorType Type => ErrorType.NotFound;
}