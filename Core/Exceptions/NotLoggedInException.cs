namespace Core.Exceptions
{
    public class NotLoggedInException : AuthException
    {
        public NotLoggedInException() : base("Not logged in")
        {
        }
    }
}