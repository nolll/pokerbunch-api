namespace Core.Errors;

public class PlayerHasGamesError() : ConflictError(("The player has played and can't be deleted"));