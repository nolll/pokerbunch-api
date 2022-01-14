namespace Core.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string userName)
        : base($"User not found: {userName}")
    {
    }
}