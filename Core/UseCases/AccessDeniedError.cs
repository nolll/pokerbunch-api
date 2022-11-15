namespace Core.UseCases;

public class AccessDeniedError : UseCaseError
{
    public AccessDeniedError()
        : this("Access denied")
    {
    }

    public AccessDeniedError(string message) : base(ErrorType.AccessDenied, message)
    {
    }
}