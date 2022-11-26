namespace Core.Errors;

public class CashgameIsPartOfEventError : ConflictError
{
    public CashgameIsPartOfEventError()
        : base("Cashgames that are part of an event can't be deleted.")
    {
    }
}