namespace Core.Exceptions
{
    public class CashgameHasResultsException : PokerBunchException
    {
        public CashgameHasResultsException()
            : base("Cashgames with results can't be deleted.")
        {
        }
    }
}