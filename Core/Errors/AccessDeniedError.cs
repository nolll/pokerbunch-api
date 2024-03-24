namespace Core.Errors;

public class AccessDeniedError(string message) : UseCaseError(message)
{
    public override ErrorType Type => ErrorType.AccessDenied;

    public AccessDeniedError() : this("Access denied")
    {
    }
}