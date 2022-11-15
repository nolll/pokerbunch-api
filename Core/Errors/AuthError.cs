namespace Core.Errors;

public class AuthError : UseCaseError
{
    public override ErrorType Type => ErrorType.Auth;

    public AuthError(string message) : base(message)
    {
    }
}