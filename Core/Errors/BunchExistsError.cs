namespace Core.Errors;

public class BunchExistsError : ConflictError
{
    public BunchExistsError(string id)
        : base($"A bunch with the id \"{id}\" already exists")
    {
    }
}