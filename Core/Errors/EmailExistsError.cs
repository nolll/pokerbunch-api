namespace Core.Errors;

public class EmailExistsError() : ConflictError("The Email Address is in use");