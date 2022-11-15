namespace Core.Errors;

public class NotLoggedInError : AuthError
{
    public NotLoggedInError() : base("Not logged in")
    {
    }
}