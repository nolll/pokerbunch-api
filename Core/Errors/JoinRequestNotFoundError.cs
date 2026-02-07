namespace Core.Errors;

public class JoinRequestNotFoundError(string id) : NotFoundError($"Join request not found: {id}");