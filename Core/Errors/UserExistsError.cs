namespace Core.Errors;

public class UserExistsError() : ConflictError("The User Name is in use");