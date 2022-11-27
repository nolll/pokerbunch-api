namespace Core.Errors;

public class InvalidJoinCodeError : ValidationError
{
    public InvalidJoinCodeError()
        : base("That code didn't work. Please check for errors and try again")
    {
    }
}