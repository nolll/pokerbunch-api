namespace Core.Errors;

public class UserNotFoundError(string userName) : NotFoundError($"User not found: {userName}");