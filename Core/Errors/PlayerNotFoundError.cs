namespace Core.Errors;

public class PlayerNotFoundError(string playerId) : NotFoundError($"Player not found: {playerId}");