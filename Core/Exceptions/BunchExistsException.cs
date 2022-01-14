namespace Core.Exceptions;

public class BunchExistsException : ConflictException
{
    public BunchExistsException(string id)
        : base($"A bunch with the id \"{id}\" already exists")
    {
    }
}