namespace Core.Entities;

public class Location(string id, string name, string bunchSlug) : IEntity
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string BunchSlug { get; } = bunchSlug;
}