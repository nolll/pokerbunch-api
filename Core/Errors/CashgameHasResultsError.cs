namespace Core.Errors;

public class CashgameHasResultsError : ConflictError
{
    public CashgameHasResultsError()
        : base("Cashgames with results can't be deleted.")
    {
    }
}