namespace Core.Entities;

public class Location : IEntity
{
    public string Id { get; }
    public string Name { get; }
    public string BunchId { get; }

    public Location(string id, string name, string bunchId)
    {
        Id = id;
        Name = name;
        BunchId = bunchId;
    }
}