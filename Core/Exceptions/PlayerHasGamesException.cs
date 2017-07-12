namespace Core.Exceptions
{
    public class PlayerHasGamesException : PokerBunchException
    {
        public PlayerHasGamesException()
            : base(("The player has played and can't be deleted"))
        {
        }
    }
}