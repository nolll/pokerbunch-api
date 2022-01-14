namespace Core.Entities;

public class Timezone
{
    public string Id { get; }
    public string Name { get; }

    public Timezone(string id, string name)
    {
        Id = id;
        Name = name;
    }
}