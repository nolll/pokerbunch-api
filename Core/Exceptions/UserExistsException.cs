namespace Core.Exceptions;

public class UserExistsException : ConflictException
{
    public UserExistsException()
        : base("The User Name is in use")
    {
    }
}