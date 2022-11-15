namespace Core.Errors;

internal class CashgameNotRunningError : ConflictError
{
    public CashgameNotRunningError()
        : base("Cashgame is not running")
    {
    }
}