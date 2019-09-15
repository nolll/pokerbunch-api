namespace Core.Exceptions
{
    public class CashgameHasResultsException : ConflictException
    {
        public CashgameHasResultsException()
            : base("Cashgames with results can't be deleted.")
        {
        }
    }
}