namespace Core.Errors;

public class AccessDeniedError : UseCaseError
{
    public override ErrorType Type => ErrorType.AccessDenied;

    public AccessDeniedError() : this("Access denied")
    {
    }

    public AccessDeniedError(string message) : base(message)
    {
    }
}