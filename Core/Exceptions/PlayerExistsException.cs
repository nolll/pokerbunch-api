namespace Core.Exceptions
{
    public class PlayerExistsException : PokerBunchException
    {
        public PlayerExistsException()
            : base(("The Display Name is in use by someone else"))
        {
        }
    }
}