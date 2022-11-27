namespace Core.Errors;

public class UserNotFoundError : NotFoundError
{
    public UserNotFoundError(string userName)
        : base($"User not found: {userName}")
    {
    }
}