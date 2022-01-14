namespace Core.Exceptions;

public class LoginException : AccessDeniedException
{
    public LoginException(string message)
        : base(message)
    {
    }
}