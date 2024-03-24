namespace Core.Entities;

public class Location(string id, string name, string bunchId) : IEntity
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string BunchId { get; } = bunchId;
}