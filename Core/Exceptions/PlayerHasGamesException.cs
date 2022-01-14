namespace Core.Exceptions;

public class PlayerHasGamesException : ConflictException
{
    public PlayerHasGamesException()
        : base(("The player has played and can't be deleted"))
    {
    }
}