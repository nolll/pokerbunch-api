namespace Core.Errors;

public abstract class NotFoundError : UseCaseError
{
    public override ErrorType Type => ErrorType.NotFound;

    protected NotFoundError(string message) : base(message)
    {
    }
}