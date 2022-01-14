namespace Core.Exceptions;

public class EmailExistsException : ConflictException
{
    public EmailExistsException()
        : base("The Email Address is in use")
    {
    }
}