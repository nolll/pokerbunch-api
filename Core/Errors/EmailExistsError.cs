namespace Core.Errors;

public class EmailExistsError : ConflictError
{
    public EmailExistsError()
        : base("The Email Address is in use")
    {
    }
}