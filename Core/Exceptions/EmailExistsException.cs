namespace Core.Exceptions
{
    public class EmailExistsException : PokerBunchException
    {
        public EmailExistsException()
            : base(("The Email Address is in use"))
        {
        }
    }
}