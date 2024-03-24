namespace Core.Errors;

public class PlayerExistsError() : ConflictError(("The Display Name is in use by someone else"));