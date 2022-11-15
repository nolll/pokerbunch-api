namespace Core.Errors;

public class LoginError : AccessDeniedError
{
    public LoginError(string message)
        : base(message)
    {
    }
}