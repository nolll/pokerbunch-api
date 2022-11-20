namespace Core.Errors;

public class PlayerNotFoundError : NotFoundError
{
    public PlayerNotFoundError(int playerId)
        : base($"Player not found: {playerId}")
    {
    }
}