namespace Core.Errors;

public class UserExistsError : ConflictError
{
    public UserExistsError()
        : base("The User Name is in use")
    {
    }
}