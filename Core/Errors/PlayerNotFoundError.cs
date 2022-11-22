namespace Core.Errors;

public class PlayerNotFoundError : NotFoundError
{
    public PlayerNotFoundError(string playerId)
        : base($"Player not found: {playerId}")
    {
    }
}