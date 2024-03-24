namespace Core.Errors;

public class AuthError(string message) : UseCaseError(message)
{
    public override ErrorType Type => ErrorType.Auth;
}