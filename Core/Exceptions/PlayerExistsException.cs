namespace Core.Exceptions;

public class PlayerExistsException : ConflictException
{
    public PlayerExistsException()
        : base(("The Display Name is in use by someone else"))
    {
    }
}