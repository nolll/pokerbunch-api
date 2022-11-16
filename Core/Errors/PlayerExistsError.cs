namespace Core.Errors;

public class PlayerExistsError : ConflictError
{
    public PlayerExistsError()
        : base(("The Display Name is in use by someone else"))
    {
    }
}