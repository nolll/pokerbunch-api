namespace Core.Errors;

public class BunchExistsError(string id) : ConflictError($"A bunch with the id \"{id}\" already exists");