namespace Core.Entities;

public class Location(string id, string name, string bunchId, string bunchSlug) : IEntity
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string BunchId { get; } = bunchId;
    public string BunchSlug { get; } = bunchSlug;
}