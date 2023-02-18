using System;

namespace Core;

public class PokerBunchException : Exception
{
    public PokerBunchException(string message) : base(message)
    {
    }
}