namespace Core.Errors;

public class PlayerHasGamesError : ConflictError
{
    public PlayerHasGamesError()
        : base(("The player has played and can't be deleted"))
    {
    }
}