namespace Core.Errors;

public class CashgameIsPartOfEventError() : ConflictError("Cashgames that are part of an event can't be deleted.");