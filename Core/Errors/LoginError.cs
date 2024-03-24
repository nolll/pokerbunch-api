namespace Core.Errors;

public class LoginError(string message) : AccessDeniedError(message);