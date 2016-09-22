using System;

namespace Core.Exceptions
{
    public class PokerBunchException : Exception
    {
        protected PokerBunchException()
        {
        }

        protected PokerBunchException(string message) : base(message)
        {
        }
    }
}